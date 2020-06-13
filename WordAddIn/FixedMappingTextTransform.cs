using System;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    /// <summary>
    /// Mapping transform that uses the same mapping for all the text.
    /// </summary>
    public class FixedMappingTextTransform : MappingTextTransform
    {
        private readonly ITextMapping mapping;

        public FixedMappingTextTransform(ITextMapping mapping)
        {
            this.mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        protected override ITextMapping GetMappingForRange(Word.Range chunk) => mapping;
    }
}
