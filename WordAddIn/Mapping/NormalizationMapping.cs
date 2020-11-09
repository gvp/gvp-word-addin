using System;

namespace GaudiaVedantaPublications
{
    /// <summary>
    /// Mapping that normalizes input string, uses inner mapping for transformation, and normalizes the result.
    /// Should be used as a wrapping for all other mappings.
    /// </summary>
    public class NormalizationMapping : ITextMapping
    {
        private readonly ITextMapping innerMapping;

        public NormalizationMapping(ITextMapping innerMapping)
        {
            this.innerMapping = innerMapping ?? throw new ArgumentNullException("innerMapping");
        }

        public ITextMapping Inverted => new NormalizationMapping(innerMapping.Inverted);

        public string Apply(string text)
        {
            return
                innerMapping.Apply(
                    text.Normalize(System.Text.NormalizationForm.FormC).PrivateUseAreaTo8Bit()
                ).Normalize(System.Text.NormalizationForm.FormC);
        }
    }
}
