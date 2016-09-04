using System;
using System.Diagnostics;
using System.Linq;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    class ToUnicodeTransform : ITextTransform
    {
        /// <summary>
        /// Only this font supports most unicode blocks. And it is required to render Devanagari correctly.
        /// </summary>
        public const string UnicodeFontName = "Arial Unicode MS";

        public void Apply(Word.Range range)
        {
            if (range == null || range.End == range.Start)
                return;

            var chunk = range.Characters.First;
            while (chunk.End <= range.End)
            {
                var nextCharacter = chunk.Next(Word.WdUnits.wdCharacter);

                /// Skipping end-of-paragraph characters.
                if (chunk.Text == "\r")
                {
                    chunk = nextCharacter;
                    continue;
                }

                if (chunk.End >= range.End || nextCharacter.Text == "\r" || !nextCharacter.Font.IsSame(chunk.Characters.First.Font))
                {
                    var map = MapManager.GetFontMap(chunk.Characters.First.Font.Name, MapDirection.Forward);
                    if (map != null)
                        chunk.Text = map.Apply(chunk.Text);

                    /// nextCharacter range could grasp current chunk due to its text replacement.
                    chunk = chunk.Next(Word.WdUnits.wdCharacter);
                }
                else
                    chunk.MoveEnd(Word.WdUnits.wdCharacter);
            }

            /// After changing the text of the last chunk original range could collapse.
            /// Restoring its End.
            range.End = Math.Max(range.End, chunk.Start);

            range.Font.Name = UnicodeFontName;
        }
    }
}
