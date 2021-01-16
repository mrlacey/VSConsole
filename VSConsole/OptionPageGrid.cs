using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace VSConsole
{
    public class OptionPageGrid : DialogPage
    {
        [DisplayName("Background")]
        [Description("The color to use for the background. Can be a named value or Hex (e.g. '#FF00FF')")]
        public string BackgroundColor { get; set; } = "Black";

        [DisplayName("Foreground")]
        [Description("The color to use for the text. Can be a named value or Hex (e.g. '#FF00FF')")]
        public string ForegroundColor { get; set; } = "LimeGreen";

        [DisplayName("FontFamily")]
        [Description("The FontFamily for the text.")]
        public string FontFamily { get; set; } = "Consolas";

        [DisplayName("TextSize")]
        [Description("The size of the text.")]
        public int FontSize { get; set; } = 12;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Messenger.RequestUpdateFormatting();
        }
    }
}
