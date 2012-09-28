using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    public interface ITextTransform
    {
        void Apply(Word.Range range);
    }
}
