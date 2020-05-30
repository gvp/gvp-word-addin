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
        private readonly IEnumerable<ITextMapping> maps;

        public MultiMapping(IEnumerable<ITextMapping> maps)
        {
            this.maps = maps;
        }

        public ITextMapping Inverted
        {
            get
            {
                return new MultiMapping(
                    (
                    from map in maps
                    let inverted = map.Inverted
                    where inverted != null
                    select inverted
                    ).Reverse()
                );
            }
        }

        public string Apply(string text)
        {
            foreach (var map in maps)
            {
                text = map.Apply(text);
            }
            return text;
        }
    }
}
