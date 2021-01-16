using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace VSConsole
{
    public class OptionPageGrid : DialogPage
    {
        public string BackgroundColor { get; set; } = "Black";

        public string ForegroundColor { get; set; } = "LawnGreen";
        
        public string FontFamily { get; set; } = "Consolas";

        public int FontSize { get; set; } = 12;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // TODO: broadcast changes
        }
    }
}
