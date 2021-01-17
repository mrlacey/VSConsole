using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using Microsoft.VisualStudio;
using System;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using System.Linq;
using System.Windows.Media;

namespace VSConsole
{
    public partial class VSCToolWindowControl : UserControl
    {
        private ITextBuffer debugTextBuffer;
        private ITrackingPoint lastTextPoint;
        private SolidColorBrush backgroundOverride;
        private SolidColorBrush foregroundOverride;
        private TextBlock currentTextBlock;

        public VSCToolWindowControl()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.InitializeComponent();

            Messenger.UpdateFormatting += () =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var options = VSCToolWindowPackage.Instance?.Options ?? new OptionPageGrid();
                this.BackgroundGrid.Background = ColorHelper.GetColorBrush(options.BackgroundColor);

                var fontFamily = new FontFamily(options.FontFamily);
                var defaultForeground = ColorHelper.GetColorBrush(options.ForegroundColor);

                foreach (var item in this.OutputWindow.Children)
                {
                    if (item is TextBlock tb)
                    {
                        tb.Foreground = defaultForeground;
                        tb.FontFamily = fontFamily;
                        tb.FontSize = options.FontSize;
                    }
                }
            };

            var opt = VSCToolWindowPackage.Instance?.Options ?? new OptionPageGrid();
            this.BackgroundGrid.Background = ColorHelper.GetColorBrush(opt.BackgroundColor);

            this.WaitForDebugOutputTextBuffer();
        }

        private bool AttachToDebugOutput()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (this.debugTextBuffer == null)
            {
                IVsOutputWindow outputWindow = (IVsOutputWindow)Package.GetGlobalService(typeof(SVsOutputWindow));
                Guid debugPaneId = VSConstants.GUID_OutWindowDebugPane;
                Guid viewId = Microsoft.VisualStudio.Editor.DefGuidList.guidIWpfTextViewHost;

                if (ErrorHandler.Succeeded(outputWindow.GetPane(ref debugPaneId, out IVsOutputWindowPane pane)) && pane is IVsUserData userData &&
                    ErrorHandler.Succeeded(userData.GetData(ref viewId, out object viewHostObject)) && viewHostObject is IWpfTextViewHost viewHost)
                {
                    this.debugTextBuffer = viewHost.TextView.TextBuffer;
                    this.debugTextBuffer.Changed += this.OnTextBufferChanged;
                    this.BeginProcessOutput();
                }
            }

            return this.debugTextBuffer != null;
        }

        /// <summary>
        /// Called for all new debug output
        /// </summary>
        private void OnTextBufferChanged(object sender, TextContentChangedEventArgs args)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            this.BeginProcessOutput();
        }

        private void BeginProcessOutput()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (this.debugTextBuffer?.CurrentSnapshot is ITextSnapshot snapshot)
            {
                ITrackingPoint startPoint = this.lastTextPoint ?? snapshot.CreateTrackingPoint(0, PointTrackingMode.Negative);
                ITrackingPoint endPoint = snapshot.CreateTrackingPoint(snapshot.Length, PointTrackingMode.Negative);
                this.lastTextPoint = endPoint;

                ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await System.Threading.Tasks.Task.Run(() => this.ProcessOutput(snapshot, startPoint, endPoint));
                }).FileAndForget(nameof(VSCToolWindow) + nameof(this.BeginProcessOutput));
            }
        }

        private void ProcessOutput(ITextSnapshot snapshot, ITrackingPoint startPoint, ITrackingPoint endPoint)
        {
            int textStart = startPoint.GetPosition(snapshot);
            int textLength = endPoint.GetPoint(snapshot) - startPoint.GetPoint(snapshot);
            string text = snapshot.GetText(textStart, textLength);

            var lines = TextToLines(text);

            var actions = new List<VSConsoleAction>();

            const string GenericStart = "VSConsole-";
            const string WriteLineStart = "VSConsole-WriteLine::";
            const string WriteStart = "VSConsole-Write::";
            const string ClearStart = "VSConsole-Clear::";
            const string ForegroundColorStart = "VSConsole-ForegroundColor::";
            const string BackgroundColorStart = "VSConsole-BackgroundColor::";
            const string ResetColorStart = "VSConsole-ResetColor::";

            foreach (var line in lines)
            {
                if (line.StartsWith(GenericStart))
                {
                    if (line.StartsWith(WriteLineStart))
                    {
                        actions.Add(VSConsoleActionType.WriteLine, line.Substring(WriteLineStart.Length));
                    }
                    else if (line.StartsWith(WriteStart))
                    {
                        actions.Add(VSConsoleActionType.Write, line.Substring(WriteStart.Length));
                    }
                    else if (line.StartsWith(ClearStart))
                    {
                        actions.Add(VSConsoleActionType.Clear);
                    }
                    else if (line.StartsWith(ForegroundColorStart))
                    {
                        actions.Add(VSConsoleActionType.SetForeground, line.Substring(ForegroundColorStart.Length));
                    }
                    else if (line.StartsWith(BackgroundColorStart))
                    {
                        actions.Add(VSConsoleActionType.SetBackground, line.Substring(BackgroundColorStart.Length));
                    }
                    else if (line.StartsWith(ResetColorStart))
                    {
                        actions.Add(VSConsoleActionType.ResetColor);
                    }
                }
            }

            if (actions.Count > 0)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    void EnsureCurrentTextBlock(bool skipColorCheck = false)
                    {
                        var options = VSCToolWindowPackage.Instance?.Options ?? new OptionPageGrid();
                        if (currentTextBlock == null)
                        {
                            currentTextBlock = new TextBlock
                            {
                                TextWrapping = TextWrapping.NoWrap,
                            };
                            this.OutputWindow.Children.Add(currentTextBlock);

                            currentTextBlock.Foreground = ColorHelper.GetColorBrush(options.ForegroundColor);
                            currentTextBlock.FontFamily = new FontFamily(options.FontFamily);
                            currentTextBlock.FontSize = options.FontSize;
                        }

                        if (backgroundOverride != null && !skipColorCheck)
                        {
                            currentTextBlock.Background = backgroundOverride;
                        }
                        else
                        {
                            currentTextBlock.Background = null;
                        }

                        if (foregroundOverride != null && !skipColorCheck)
                        {
                            currentTextBlock.Foreground = foregroundOverride;
                        }
                        else
                        {
                            currentTextBlock.Foreground = ColorHelper.GetColorBrush(options.ForegroundColor);
                        }
                    }

                    foreach (var vsaction in actions)
                    {
                        switch (vsaction.ActionType)
                        {
                            case VSConsoleActionType.WriteLine:
                                EnsureCurrentTextBlock(string.IsNullOrEmpty(vsaction.Value));
                                currentTextBlock.Text += $"{vsaction.Value}";
                                currentTextBlock = null;
                                break;
                            case VSConsoleActionType.Write:
                                EnsureCurrentTextBlock();
                                currentTextBlock.Text += $"{vsaction.Value}";
                                break;
                            case VSConsoleActionType.Clear:
                                this.OutputWindow.Children.Clear();
                                break;
                            case VSConsoleActionType.SetForeground:
                                foregroundOverride = ColorHelper.GetColorBrush(vsaction.Value);
                                break;
                            case VSConsoleActionType.SetBackground:
                                backgroundOverride = ColorHelper.GetColorBrush(vsaction.Value);
                                break;
                            case VSConsoleActionType.ResetColor:
                                backgroundOverride = null;
                                foregroundOverride = null;
                                break;
                            default:
                                break;
                        }
                    }
                }).FileAndForget(nameof(VSCToolWindow) + nameof(this.ProcessOutput));
            }
        }

        private void WaitForDebugOutputTextBuffer()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (!this.AttachToDebugOutput())
            {
                //CancellationToken cancelToken = this.cancellationTokenSource.Token;

                ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    //while (!this.AttachToDebugOutput() && this.viewModel.IsDebugging && !cancelToken.IsCancellationRequested)
                    while (!this.AttachToDebugOutput())
                    {
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1.0));
                    }
                }).FileAndForget(nameof(VSCToolWindow) + nameof(this.WaitForDebugOutputTextBuffer));
            }
        }

        public IEnumerable<string> TextToLines(string input)
            => input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(a => !string.IsNullOrEmpty(a));

        private void OnClearClicked(object sender, RoutedEventArgs e)
            => this.OutputWindow.Children.Clear();
    }
}
