using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using Microsoft.VisualStudio;
using System;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using System.Linq;
using System.Threading;

namespace VSConsole
{
    /// <summary>
    /// Interaction logic for VSCToolWindowControl.
    /// </summary>
    public partial class VSCToolWindowControl : UserControl
    {
        private ITextBuffer debugTextBuffer;
        private ITrackingPoint lastTextPoint;

        public VSCToolWindowControl()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.InitializeComponent();

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

            var linesToAdd = new List<string>();

            foreach (var line in lines)
            {
                if (line.StartsWith("VSConsole-"))
                {
                    linesToAdd.Add(line);
                }
            }

            if (linesToAdd.Count > 0)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    foreach (var line in linesToAdd)
                    {
                        this.OutputWindow.Text += $"{line}{Environment.NewLine}";
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
            => this.OutputWindow.Text = string.Empty;
    }
}
