using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaudiaVedantaPublications
{
    public class ReplaceMapping : ITextMapping
    {
        private readonly string from;
        private readonly string to;

        public ReplaceMapping(string from, string to)
        {
            this.from = from;
            this.to = to;
        }

        public ITextMapping Inverted
        {
            get
            {
                return new ReplaceMapping(to, from);
            }
        }

        public string Apply(string text)
        {
#if TRACE_TEXT_TRANSFORMATION
            var oldText = text;
#endif
            text = text.Replace(from, to);
#if TRACE_TEXT_TRANSFORMATION
            if (oldText != text)
                System.Diagnostics.Trace.TraceInformation("'{0}' → '{1}'\twith {2}", oldText, text, this);
#endif
            return text;
        }

        public override string ToString()
        {
            return String.Format("'{0}' → '{1}'", from, to);
        }
    }
}
