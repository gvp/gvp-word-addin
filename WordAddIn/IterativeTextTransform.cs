using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    abstract class IterativeTextTransform : ITextTransform
    {
        public virtual void Apply(Word.Range range)
        {
            Word.Range lastCharacter = null;
            for (
                var character = range.Characters.First;
                character != null && character.End <= range.End;
                character = character.Next()
                )
            {
                TransformCharacter(character);
                lastCharacter = character;
            }

            /// Address an inappropriate behaviour when changing the last character makes range exclude it.
            range.End = lastCharacter.End;
        }

        protected abstract void TransformCharacter(Word.Range character);
    }
}
