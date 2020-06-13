using System;
using System.Linq;
using System.Collections.Generic;

namespace GaudiaVedantaPublications
{
    /// <summary>
    /// Combines several mappings and applies them consequently.
    /// </summary>
    public class MultiMapping : ITextMapping
    {
        private readonly IEnumerable<ITextMapping> mappings;

        public MultiMapping(IEnumerable<ITextMapping> mappings)
        {
            this.mappings = mappings;
        }

        public ITextMapping Inverted
        {
            get
            {
                return new MultiMapping(
                    (
                    from map in mappings
                    let inverted = map.Inverted
                    where inverted != null
                    select inverted
                    ).Reverse()
                );
            }
        }

        public string Apply(string text)
        {
            foreach (var map in mappings)
            {
                text = map.Apply(text);
            }
            return text;
        }
    }
}
