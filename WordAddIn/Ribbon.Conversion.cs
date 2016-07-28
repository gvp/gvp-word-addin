using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Core;

namespace GaudiaVedantaPublications
{
    partial class Ribbon
    {
        private static readonly string[] CyrillicFontNames = { "ThamesM" };
        private static readonly string[] RomanFontNames = { "ScaTimes", "Rama Garamond Plus", "GVPalatino" };

        public string GetFontConversionLabel(IRibbonControl control)
        {
            /// For specific font we extract the font name from the tag.
            if (!String.IsNullOrWhiteSpace(control.Tag))
                return GetFontConversionLabel(control.Tag);

            /// For default button we extract the font name from OperationalFontName setting.
            if (!String.IsNullOrWhiteSpace(OperationalFontName))
                return GetFontConversionLabel(OperationalFontName);

            /// If operational font is not yet set, we show general title.
            return GetLabel(control);
        }

        public bool GetOperationalFontIsSet(IRibbonControl control)
        {
            return !String.IsNullOrWhiteSpace(OperationalFontName);
        }

        public void ClearOperationalFont(IRibbonControl control)
        {
            OperationalFontName = null;
        }

        private string GetFontConversionLabel(String fontName)
        {
            return String.Format(Properties.Resources.ConvertToFont_Label, fontName);
        }

        public void ConvertToUnicode(IRibbonControl conrtol)
        {
            Globals.ThisAddIn.TransformText(new ToUnicodeTransform());
        }

        public void ConvertFont(IRibbonControl control)
        {
            if (!String.IsNullOrWhiteSpace(control.Tag))
                OperationalFontName = control.Tag;
            ConvertFont(OperationalFontName);
        }

        private static void ConvertFont(String fontName)
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
