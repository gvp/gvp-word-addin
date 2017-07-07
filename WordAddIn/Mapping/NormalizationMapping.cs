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
            if (innerMapping == null)
                throw new ArgumentNullException("innerMapping");

            this.innerMapping = innerMapping;
        }

        public ITextMapping Inverted
        {
            get
            {
                return new NormalizationMapping(innerMapping.Inverted);
            }
        }

        public string Apply(string text)
        {
            return 
                innerMapping.Apply(
                    text.Normalize(System.Text.NormalizationForm.FormC).PrivateUseAreaToAnsi()
                ).Normalize(System.Text.NormalizationForm.FormC);
        }
    }
}
