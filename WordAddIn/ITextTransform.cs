using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    public interface ITextTransform
    {
        void Apply(Word.Range range);
    }
}
