using System.Collections.Generic;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    internal class CombiningTransform : ITextTransform
    {
        private readonly IEnumerable<ITextTransform> transforms;

        public CombiningTransform(params ITextTransform[] transforms)
        {
            this.transforms = transforms;
        }

        public void Apply(Word.Range range)
        {
            foreach (var transform in transforms)
                transform.Apply(range);
        }
    }
}
