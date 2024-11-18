using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace VSConsole
{
	public class OutputPane
	{
		private static Guid vscPaneGuid = new Guid("50AD8075-1DA7-4D6A-A6D7-7F6698D6AC03");

		private static OutputPane instance;

		private readonly IVsOutputWindowPane pane;

		private OutputPane()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			if (ServiceProvider.GlobalProvider.GetService(typeof(SVsOutputWindow)) is IVsOutputWindow outWindow
			 && (ErrorHandler.Failed(outWindow.GetPane(ref vscPaneGuid, out pane)) || pane == null))
			{
				if (ErrorHandler.Failed(outWindow.CreatePane(ref vscPaneGuid, Vsix.Name, 1, 0)))
				{
					System.Diagnostics.Debug.WriteLine("Failed to create the Output window pane.");
					return;
				}

				if (ErrorHandler.Failed(outWindow.GetPane(ref vscPaneGuid, out pane)) || (pane == null))
				{
					System.Diagnostics.Debug.WriteLine("Failed to get access to the Output window pane.");
				}
			}
		}

		public static OutputPane Instance => instance ??= new OutputPane();

		public async Task ActivateAsync()
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(CancellationToken.None);

			pane?.Activate();
		}

		public async Task WriteAsync(string message)
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(CancellationToken.None);

			_ = (pane?.OutputStringThreadSafe($"{message}{Environment.NewLine}"));
		}
	}
}
