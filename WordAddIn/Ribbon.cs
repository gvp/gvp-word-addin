using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    [ComVisible(true)]
    public class Ribbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;
        private int selectedFontIndex;
        private bool devanagari2roman;
        private bool roman2cyrillic;

        private static IDictionary<string, string> FontMap = new Dictionary<string, string>
        {
            { "fontItemGVPalatino", "GVPalatino" }
        };

        #region FontNames

        private static IEnumerable<string> FontNames
        {
            get
            {
                yield return Properties.Resources.NormalFontName;

                if (RussianOptions)
                    yield return "ThamesM";

                yield return "Rama Garamond Plus";
                yield return "GVPalatino";
                yield return "ScaTimes";
            }
        }

        private string SelectedFontName
        {
            get
            {
                return FontNames.ElementAt(selectedFontIndex);
            }
        }

        #endregion

        public Ribbon()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return EmbeddedResourceManager.GetEmbeddedStringResource("Ribbon.xml");
        }

        #endregion


        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, select the Ribbon XML item in Solution Explorer and then press F1

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public string GetLabel(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Label");
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

        // *** Fonts ********************************************
        public int GetFontsCount(IRibbonControl control)
        {
            return FontNames.Count();
        }

        public string GetFontItemName(IRibbonControl control, int index)
        {
            return FontNames.ElementAt(index);
        }

        public int GetSelectedFontIndex(IRibbonControl control)
        {
            return selectedFontIndex;
        }

        public void SetSelectedFont(IRibbonControl control, string selectedId, int selectedIndex)
        {
            selectedFontIndex = selectedIndex;
        }

        // *** Checkboxes ********************************************

        public void SetDevanagari2Roman(IRibbonControl control, bool @checked)
        {
            devanagari2roman = @checked;
        }

        public void SetRoman2Cyrillic(IRibbonControl control, bool @checked)
        {
            roman2cyrillic = @checked;
        }

        // *** Russian options ********************************************

        public bool GetRussianOptionsState(IRibbonControl control)
        {
            return RussianOptions;
        }

        public void SetRussianOptionsState(IRibbonControl control, bool pressed)
        {
            var savedFontName = SelectedFontName;
            RussianOptions = pressed;
            roman2cyrillic = false;
            selectedFontIndex = Math.Max(0, FontNames.ToList().IndexOf(savedFontName));
            ribbon.Invalidate();
        }

        public void Process(IRibbonControl control)
        {
            Globals.ThisAddIn.TransformText(GetTransforms().ToArray());
        }

        #endregion

        #region Helpers

        private IEnumerable<ITextTransform> GetTransforms()
        {
            yield return new ToUnicodeTransform();
            if (devanagari2roman)
                yield return new DevanagariTransliterationTransform();

            if (roman2cyrillic)
                yield return new MapBasedTextTransform(MapManager.Lat2Cyr);

            if (selectedFontIndex > 0)
                yield return new FromUnicodeTransform(FontNames.ElementAt(selectedFontIndex));
        }

        private static bool RussianOptions
        {
            get
            {
                return Properties.Settings.Default.RussianOptions;
            }
            set
            {
                Properties.Settings.Default.RussianOptions = value;
                Properties.Settings.Default.Save();
            }
        }

        #endregion
    }
}
