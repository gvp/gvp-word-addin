using System.Collections.Generic;
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
    }
}
