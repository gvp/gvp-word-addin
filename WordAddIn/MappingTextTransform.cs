using System;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    public abstract class MappingTextTransform : ITextTransform
    {
        protected abstract ITextMapping GetMappingForRange(Word.Range chunk);

        protected virtual string GetNewFont(Word.Range range) => null;

        public virtual void Apply(Word.Range range)
        {
            if (range == null || range.End == range.Start)
                return;

            /// Building chunks of the same-font characters.
            /// WdUnits.wdCharacterFormatting does not work well.
            var chunk = range.Characters.First;
            while (chunk.End <= range.End)
            {
                var nextCharacter = chunk.Next(Word.WdUnits.wdCharacter);

                /// Skipping end-of-paragraph characters.
                if (chunk.Text == "\r")
                {
                    if (nextCharacter == null)
                        break;
                    chunk = nextCharacter;
                    continue;
                }

                if (chunk.End >= range.End || nextCharacter.Text == "\r" || ShouldSplit(chunk, nextCharacter))
                {
                    var mapping = GetMappingForRange(chunk);
                    if (mapping != null)
                    {
                        var newFontName = GetNewFont(chunk);
                        var text = mapping.Apply(chunk.Text);
#if TRANSFORMATION_COMPARISON
                        chunk.InsertAfter("\n");
                        chunk.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
#endif
                        chunk.Text = text;
                        chunk.Font.Name = newFontName;
#if TRANSFORMATION_COMPARISON
                        chunk.HighlightColorIndex = Word.WdColorIndex.wdYellow;
#endif
                    }

                    /// nextCharacter range could grasp current chunk due to its text replacement.
                    chunk = chunk.Next(Word.WdUnits.wdCharacter);
                }
                else
                    chunk.MoveEnd(Word.WdUnits.wdCharacter);
            }

            /// After changing the text of the last chunk original range could collapse.
            /// Restoring its End.
            range.End = Math.Max(range.End, chunk.Start);
        }

        protected virtual bool ShouldSplit(Word.Range first, Word.Range second)
        {
            return !second.Font.IsSame(first.Characters.First.Font);
        }
    }
}
