using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    abstract class IterativeTextTransform : ITextTransform
    {
        public virtual void Apply(Word.Range range)
        {
            var character = range.Characters.First;
            var last = range.Characters.Last.Duplicate;

            while (character != null && character.End <= last.End)
            {
                TransformCharacter(character);

                /// Range.Next jumps over the next unit if positioned right before it.
                /// So, handling collapsed ranges especially.
                if (character.Start == character.End)
                    character.MoveEnd(1);
                else
                    character = character.Next();
            }

            /// Address an inappropriate behaviour when changing the last character makes range exclude it.
            range.End = character.Start;
        }

        protected abstract void TransformCharacter(Word.Range character);
    }
}
