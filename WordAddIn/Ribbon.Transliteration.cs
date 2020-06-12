using System;
using Microsoft.Office.Core;

namespace GaudiaVedantaPublications
{
    partial class Ribbon
    {
        public void Transliterate(IRibbonControl control)
        {
            Globals.ThisAddIn.TransformText(new ToUnicodeTransform(), GetTransliterationTransform(control));
        }

        private ITextTransform GetTransliterationTransform(IRibbonControl control)
        {
            switch (control.Id)
            {
                case "Devanagari2Latin":
                    return new FixedMappingTextTransform(MappingManager.DevanagariToLatin);

                case "Latin2Cyrillic":
                    return new FixedMappingTextTransform(MappingManager.LatinToCyrillic);

                default:
                    throw new ArgumentException(string.Format("Unsupported transliteration: {0}", control.Id), nameof(control));
            }
        }
    }
}
