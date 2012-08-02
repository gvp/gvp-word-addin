using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    abstract class IterativeTextTransform : ITextTransform
    {
        public virtual void Apply(Word.Range range)
        {
            var end = range.End;
            CurrentCharacter = range.Characters.First;
            while (CurrentCharacter != null && CurrentCharacter.End <= end)
            {
                var passed = CurrentCharacter.End - range.Start;

                TransformCharacter();

                /// Range.Next() jumps over the next unit if positioned right before it.
                CurrentCharacter.EndOf(Word.WdUnits.wdCharacter, Word.WdMovementType.wdMove);

                /// Correct end position.
                end += (CurrentCharacter.End - range.Start) - passed;

                /// Span the next character
                CurrentCharacter.EndOf(Word.WdUnits.wdCharacter, Word.WdMovementType.wdExtend);
            }

            /// Address an inappropriate behaviour when changing the last character makes range exclude it.
            range.End = end;
        }

        protected Word.Range CurrentCharacter { get; private set; }

        protected abstract void TransformCharacter();
    }
}
