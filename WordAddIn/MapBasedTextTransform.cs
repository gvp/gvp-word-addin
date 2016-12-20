using System;
using System.Linq;
using System.Text;
using Map = System.Linq.IOrderedEnumerable<GaudiaVedantaPublications.MapEntry>;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    public class MapBasedTextTransform : ITextTransform
    {
        /// <summary>
        /// This contructor is used with descendant classes where GetMapForRange is overriden.
        /// </summary>
        protected MapBasedTextTransform()
        {
        }

        /// <summary>
        /// This constructor is used for external instantiating the transform with a single map.
        /// </summary>
        /// <param name="map">A map that is used for all transformations.</param>
        public MapBasedTextTransform(Map map)
        {
            this.map = map;
        }

        private readonly Map map;

        protected virtual Map GetMapForRange(Word.Range chunk)
        {
            if (map == null)
                throw new InvalidOperationException("Map is not set");

            return map;
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
                    var map = GetMapForRange(chunk);
                    if (map != null && map.Any())
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
        }

        protected virtual bool ShouldSplit(Word.Range first, Word.Range second)
        {
            return !second.Font.IsSame(first.Characters.First.Font);
        }
    }
}
