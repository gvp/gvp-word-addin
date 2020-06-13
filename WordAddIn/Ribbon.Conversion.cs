using Microsoft.Office.Core;

namespace GaudiaVedantaPublications
{
    public partial class Ribbon
    {
        public string GetFontConversionLabel(IRibbonControl control)
        {
            /// For specific font we extract the font name from the tag.
            if (!string.IsNullOrWhiteSpace(control.Tag))
                return GetFontConversionLabel(control.Tag);

            /// For default button we extract the font name from OperationalFontName setting.
            if (!string.IsNullOrWhiteSpace(OperationalFontName))
                return GetFontConversionLabel(OperationalFontName);

            /// If operational font is not yet set, we show general title.
            return GetLabel(control);
        }

        public bool GetOperationalFontIsSet(IRibbonControl control)
        {
            return !string.IsNullOrWhiteSpace(OperationalFontName);
        }

        public void ClearOperationalFont(IRibbonControl control)
        {
            OperationalFontName = null;
        }

        private string GetFontConversionLabel(string fontName)
        {
            return string.Format(Properties.Resources.ConvertToFont_Label, fontName);
        }

        public void ConvertToUnicode(IRibbonControl conrtol)
        {
            Globals.ThisAddIn.TransformText(new ToUnicodeTransform());
        }

        public void ConvertFromUnicode(IRibbonControl control)
        {
            if (!string.IsNullOrWhiteSpace(control.Tag))
                OperationalFontName = control.Tag;
            ConvertFromUnicode(OperationalFontName);
        }

        private static void ConvertFromUnicode(string fontName)
        {
            Globals.ThisAddIn.TransformText(new ToUnicodeTransform(), new FromUnicodeTransform(fontName));
        }

        private string OperationalFontName
        {
            get
            {
                var fontName = Properties.Settings.Default.OperationalFontName;
                return fontName;
            }
            set
            {
                Properties.Settings.Default.OperationalFontName = value;
                Properties.Settings.Default.Save();
                ribbon.InvalidateControl("ConvertToOperationalFont");
            }
        }
    }
}
