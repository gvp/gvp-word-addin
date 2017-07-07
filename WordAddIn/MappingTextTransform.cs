using System;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    public class MappingTextTransform : ITextTransform
    {
        /// <summary>
        /// This contructor is used with descendant classes where GetMappingForRange is overriden.
        /// </summary>
        protected MappingTextTransform()
        {
        }

        /// <summary>
        /// This constructor is used for external instantiating the transform with a single map.
        /// </summary>
        /// <param name="map">A map that is used for all transformations.</param>
        public MappingTextTransform(ITextMapping mapping)
        {
            this.mapping = mapping;
        }

        private readonly ITextMapping mapping;

        protected virtual ITextMapping GetMappingForRange(Word.Range chunk)
        {
            if (mapping == null)
                throw new InvalidOperationException("Mapping is not set");

            return mapping;
        }

        protected virtual void PostProcess(Word.Range range)
        {
        }

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
                        var text = mapping.Apply(chunk.Text);
#if TRANSFORMATION_COMPARISON
                        chunk.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
#endif
                        chunk.Text = text;
                        PostProcess(chunk);
#if TRANSFORMATION_COMPARISON
                        chunk.Font.Color = Word.WdColor.wdColorRed;
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
