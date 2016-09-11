﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Core;

namespace GaudiaVedantaPublications
{
    partial class Ribbon
    {
        public void Transliterate(IRibbonControl control)
        {
            Globals.ThisAddIn.TransformText(GetTransliterationTransforms(control).ToArray());
        }

        private IEnumerable<ITextTransform> GetTransliterationTransforms(IRibbonControl control)
        {
            yield return new ToUnicodeTransform();
            switch (control.Id)
            {
                case "TransliterateDevanagari":
                    yield return new MapBasedTextTransform(MapManager.Dev2Lat);
                    break;

                case "TransliterateRoman":
                    yield return new MapBasedTextTransform(MapManager.Lat2Cyr);
                    yield return new FromUnicodeTransform(CyrillicFontNames.First());
                    break;
            }
        }
    }
}
