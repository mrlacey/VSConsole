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
        DTE _dte;
        Events _dteEvents;
        OutputWindowEvents _documentEvents;

        private ITextBuffer debugTextBuffer;
        private ITrackingPoint lastTextPoint;

        public VSCToolWindowControl()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.InitializeComponent();

            //_dte = (DTE)Package.GetGlobalService(typeof(SDTE));
            //_dteEvents = _dte.Events;
            //_documentEvents = _dteEvents.OutputWindowEvents;
            //_documentEvents.PaneUpdated += PaneUpdated;

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

            //List<IReadOnlyList<ITableEntry>> allEntries = new List<IReadOnlyList<ITableEntry>>(this.outputParsers.Count);

            //foreach (IOutputParser outputParser in this.outputParsers)
            //{
            //    IReadOnlyList<ITableEntry> entries = outputParser.ParseOutput(text);
            //    if (entries.Count > 0)
            //    {
            //        allEntries.Add(entries);
            //    }
            //}

            if (linesToAdd.Count > 0)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async delegate
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    //bool added = false;

                    //foreach (IReadOnlyList<ITableEntry> entries in allEntries)
                    //{
                    //    if (this.viewModel.AddEntries(entries))
                    //    {
                    //        added = true;
                    //    }
                    //}

                    //if (added)
                    //{
                    //    this.NotifyUserAboutNewEntries();
                    //}

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



        private void PaneUpdated(OutputWindowPane pane)
        {
            //if (!WindowList.Contains(pane.Name))
            //{
            //    WindowList.Add(pane.Name);
            //    if (string.IsNullOrEmpty(CurrentWindow)) CurrentWindow = pane.Name;
            //}

            //if (!_windowNames.ContainsKey(pane.Name)) _windowNames.Add(pane.Name, pane.Guid);

            // See [IDE GUID](https://docs.microsoft.com/en-us/visualstudio/extensibility/ide-guids?view=vs-2017 )
            ProcessNewInput(pane);

            //UpdateOutput();
        }

        public void ProcessNewInput(OutputWindowPane pane)
        {
            //if (!_outputWindowContent.ContainsKey(pane.Name)) _outputWindowContent.Add(pane.Name, new List<PaneContentLineModel>());
            //var currentPaneContent = _outputWindowContent[pane.Name];

            var newPaneContent = GetPaneData(pane);

            //if (currentPaneContent.Count() > newPaneContent.Count())
            //{
            //    //pane cleared process all again
            //    currentPaneContent = newPaneContent.Select(z => new PaneContentLineModel { Text = z }).ToList();
            //    currentPaneContent.ForEach(line => {
            //        line.MatchesFilter = FilterMode == FilteringMode.Include ? Expression(line.Text) : Expression.Not()(line.Text);

            //    });

            //}
            //else
            //{
            //    var newLines = newPaneContent.Skip(currentPaneContent.Count()).Select(z => new PaneContentLineModel { Text = z }).ToList();
            //    currentPaneContent.ForEach(line => {
            //        line.MatchesFilter = FilterMode == FilteringMode.Include ? Expression(line.Text) : Expression.Not()(line.Text);

            //    });
            //    currentPaneContent.AddRange(newLines);
            //}
        }

        private IEnumerable<string> GetPaneData(OutputWindowPane pPane)
        {
            TextDocument document = pPane.TextDocument;
            EditPoint point = document.StartPoint.CreateEditPoint();
            var text = point.GetText(document.EndPoint);
            return TextToLines(text);
        }

        public IEnumerable<string> TextToLines(string input)
        {
            return input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(a => !string.IsNullOrEmpty(a));
        }


        private void OnClearClicked(object sender, RoutedEventArgs e)
        {
            this.OutputWindow.Text = string.Empty;
        }
    }
}