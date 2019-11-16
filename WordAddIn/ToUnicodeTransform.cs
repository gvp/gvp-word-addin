using System;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    public class ToUnicodeTransform : MappingTextTransform
    {
        /// <summary>
        /// Only this font supports most unicode blocks. And it is required to render Devanagari correctly.
        /// </summary>
        public const string DefaultUnicodeFontName = "Arial Unicode MS";

        protected override ITextMapping GetMappingForRange(Word.Range range)
        {
            var font = range.Characters.First.Font;
            if (string.IsNullOrEmpty(font.Name))
                throw new InvalidOperationException("Range contains several fonts");

            return MappingManager.GetFontToUnicodeMapping(font.Name);
        }

        protected override bool ShouldSplit(Word.Range first, Word.Range second)
        {
            /// If font is exactly the same, then no split.
            if (!base.ShouldSplit(first, second))
                return false;

            /// We should not split if second range is in the same word
            /// and the font is of the same name.
            if (second.Text[0] != ' ' && second.Font.Name == first.Font.Name)
                return false;

            /// Font name is different
            return true;
        }

        protected override string GetNewFont(Word.Range range)
        {
            var unicodeFontName = FindUnicodeFontName(range.Characters.First.Font.Name);
            if (!range.Application.FontNames.OfType<string>().Contains(unicodeFontName))
                return DefaultUnicodeFontName;

            return unicodeFontName;
        }

        private string FindUnicodeFontName(string fontName)
        {
            switch (fontName)
            {
                case "AARituPlus2":
                case "AARituPlus2-Numbers":
                case "AARitu":
                case "AAVishal":
                case "Mangal":
                    return "AARUPA";

                default:
                    return DefaultUnicodeFontName;
            }
        }
    }
}
