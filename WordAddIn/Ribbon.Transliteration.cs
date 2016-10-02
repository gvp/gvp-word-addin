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
                case "Devanagari2Latin":
                    yield return new MapBasedTextTransform(MapManager.DevanagariToLatin);
                    break;

                case "Latin2Cyrillic":
                    yield return new MapBasedTextTransform(MapManager.LatinToCyrillic);
                    break;
            }
        }
    }
}
