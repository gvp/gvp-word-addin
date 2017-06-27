using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaudiaVedantaPublications
{
    public class ReplaceMap : Map
    {
        private readonly string from;
        private readonly string to;

        public string From
        {
            get { return from; }
        }

        public string To
        {
            get { return to; }
        }

        public ReplaceMap(string from, string to)
        {
            this.from = from;
            this.to = to;
        }

        public override Map Inverted
        {
            get
            {
                return new ReplaceMap(to, from);
            }
        }

        public override string Apply(string text)
        {
            return text.Replace(from, to);
        }

        public override string ToString()
        {
            return String.Format("'{0}' → '{1}'", from, to);
        }
    }
}
