using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using Office = Microsoft.Office.Core;

namespace GaudiaVedantaPublications
{
    [ComVisible(true)]
    public class Ribbon : Office.IRibbonExtensibility
    {
        private static readonly string[] CyrillicFontNames = { "ThamesM" };
        private static readonly string[] RomanFontNames = { "ScaTimes", "Rama Garamond Plus", "GVPalatino" };

        private Office.IRibbonUI ribbon;

        public Ribbon()
        {
        }

        public string GetCustomUI(string ribbonID)
        {
            return EmbeddedResourceManager.GetEmbeddedStringResource("Ribbon.xml");
        }

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, select the Ribbon XML item in Solution Explorer and then press F1

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public string GetFontConversionLabel(IRibbonControl control)
        {
            var fontName = String.IsNullOrWhiteSpace(control.Tag) ? OperationalFontName : control.Tag;
            return String.Format(Properties.Resources.ConvertToFont_Label, fontName);
        }

        public string GetLabel(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Label");
        }

        public string GetTitle(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Title");
        }

        public string GetDescription(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Description");
        }

        public string GetScreentip(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Screentip");
        }

        public string GetSupertip(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Supertip");
        }

        private static string GetResourceString(string id, string attribute)
        {
            return Properties.Resources.ResourceManager.GetString(String.Format("{0}_{1}", id, attribute));
        }

        // *** Cyrillic options ********************************************

        public bool GetCyrillicOptionsState(IRibbonControl control)
        {
            return CyrillicOptions;
        }

        public void SetCyrillicOptionsState(IRibbonControl control, bool pressed)
        {
            CyrillicOptions = pressed;
            ribbon.Invalidate();
        }

        public void Process(IRibbonControl control)
        {
            Transform(control.Id);
        }

        public void ConvertFont(IRibbonControl control)
        {
            if (!String.IsNullOrWhiteSpace(control.Tag))
            {
                OperationalFontName = control.Tag;
                ribbon.InvalidateControl("ConvertToOperationalFont");
            }
            Transform("ConvertToOperationalFont");
        }

        private void Transform(String mode)
        {
            Globals.ThisAddIn.TransformText(GetTransforms(mode).ToArray());
        }

        private IEnumerable<ITextTransform> GetTransforms(String mode)
        {
            yield return new ToUnicodeTransform();

            switch (mode)
            {
                case "ConvertToOperationalFont":
                    yield return new FromUnicodeTransform(OperationalFontName);
                    break;

                case "TransliterateDevanagari":
                    yield return new DevanagariTransliterationTransform();
                    if (RomanFontNames.Contains(OperationalFontName))
                        yield return new FromUnicodeTransform(OperationalFontName);
                    break;

                case "TransliterateRoman":
                    yield return new MapBasedTextTransform(MapManager.Lat2Cyr);
                    yield return new FromUnicodeTransform(CyrillicFontNames.First());
                    break;
            }
        }

        #endregion


        #region *************** Settings *****************

        private static bool CyrillicOptions
        {
            get
            {
                return Properties.Settings.Default.CyrillicOptions;
            }
            set
            {
                Properties.Settings.Default.CyrillicOptions = value;
                Properties.Settings.Default.Save();
            }
        }

        private static string OperationalFontName
        {
            get
            {
                var fontName = Properties.Settings.Default.OperationalFontName;
                if (String.IsNullOrWhiteSpace(fontName))
                    fontName = CyrillicFontNames.FirstOrDefault();

                if (CyrillicFontNames.Contains(fontName) && !CyrillicOptions)
                    fontName = RomanFontNames.FirstOrDefault();

                return fontName;
            }
            set
            {
                Properties.Settings.Default.OperationalFontName = value;
                Properties.Settings.Default.Save();
            }
        }

        #endregion
    }
}
