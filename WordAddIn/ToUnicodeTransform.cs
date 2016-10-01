using System;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    public class ToUnicodeTransform : MapBasedTextTransform
    {
        /// <summary>
        /// Only this font supports most unicode blocks. And it is required to render Devanagari correctly.
        /// </summary>
        public const string UnicodeFontName = "Arial Unicode MS";

        public override void Apply(Word.Range range)
        {
            base.Apply(range);
            range.Font.Name = UnicodeFontName;
        }

        protected override IOrderedEnumerable<MapEntry> GetMapForRange(Word.Range range)
        {
            var font = range.Characters.First.Font;
            if (string.IsNullOrEmpty(font.Name))
                throw new InvalidOperationException("Range contains several fonts");

            return MapManager.GetFontToUnicodeMap(font.Name);
        }
    }
}
