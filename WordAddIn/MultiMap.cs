using System;
using System.Collections.Generic;

namespace GaudiaVedantaPublications
{
    public class MultiMap : Map
    {
        private readonly IEnumerable<Map> maps;

        public MultiMap(IEnumerable<Map> maps)
        {
            this.maps = maps;
        }

        public override Map Inverted
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Apply(string text)
        {
            foreach (var map in maps)
            {
                text = map.Apply(text);
            }
            return text;
        }
    }
}
