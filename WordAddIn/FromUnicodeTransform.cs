﻿using System;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    class FromUnicodeTransform : MapBasedTextTransform
    {
        private readonly String fontName;

        public FromUnicodeTransform(String fontName)
            : base(MapManager.GetUnicodeToFontMap(fontName))
        {
            if (fontName == null)
                throw new ArgumentNullException("toFontName");
            this.fontName = fontName;
        }

        public override void Apply(Word.Range range)
        {
            base.Apply(range);
            range.Font.Name = fontName;
            if (fontName == "ThamesM")
            {
                range.Font.Italic = 1;
                range.Font.Bold = 0;
            }
        }

    }
}
