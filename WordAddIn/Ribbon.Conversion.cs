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
            if (!String.IsNullOrWhiteSpace(control.Tag))
                return GetFontConversionLabel(control.Tag);

            if (!String.IsNullOrWhiteSpace(OperationalFontName))
                return GetFontConversionLabel(OperationalFontName);

            return Properties.Resources.ConvertToUnicode_Label;
        }

        private string GetFontConversionLabel(String fontName)
        {
            return String.Format(Properties.Resources.ConvertToFont_Label, fontName);
        }

        public void ConvertToUnicode(IRibbonControl conrtol)
        {
            OperationalFontName = null;
            ConvertFont(OperationalFontName);
        }

        public void ConvertFont(IRibbonControl control)
        {
            if (!String.IsNullOrWhiteSpace(control.Tag))
                OperationalFontName = control.Tag;
            ConvertFont(OperationalFontName);
        }

        private static void ConvertFont(String fontName)
        {
            Globals.ThisAddIn.TransformText(GetTransformsForFont(fontName).ToArray());
        }

        private static IEnumerable<ITextTransform> GetTransformsForFont(String fontName)
        {
            yield return new ToUnicodeTransform();
            if (!String.IsNullOrWhiteSpace(fontName))
                yield return new FromUnicodeTransform(fontName);
        }

        private string OperationalFontName
        {
            get
            {
                var fontName = Properties.Settings.Default.OperationalFontName;
                if (CyrillicFontNames.Contains(fontName) && !CyrillicOptions)
                    fontName = RomanFontNames.FirstOrDefault();

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
