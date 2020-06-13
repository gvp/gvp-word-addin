using System;
using System.Text.RegularExpressions;

namespace GaudiaVedantaPublications
{
    /// <summary>
    /// Mapping that uses <see cref="Regex.Replace"/> method.
    /// </summary>
    public class RegexMapping : ITextMapping
    {
        protected readonly Regex regex;
        private readonly string replacement;

        /// <summary>
        /// Constructor for basic regex replacement.
        /// </summary>
        /// <param name="pattern">Pattern for searching.</param>
        /// <param name="replacement">A string to replace with.</param>
        public RegexMapping(string pattern, string replacement)
        {
            this.regex = new Regex(pattern, RegexOptions.Compiled);
            this.replacement = replacement;
        }

        public ITextMapping Inverted
        {
            get
            {
                throw new InvalidOperationException("Cannot invert regex-based map");
            }
        }

        public string Apply(string text)
        {
#if TRACE_TEXT_TRANSFORMATION
            var oldText = text;
#endif
            text = regex.Replace(text, replacement);
#if TRACE_TEXT_TRANSFORMATION
            if (oldText != text)
                System.Diagnostics.Trace.TraceInformation("'{0}' → '{1}'\twith {2}", oldText, text, this);
#endif
            return text;
        }

        public override string ToString()
        {
            return String.Format("'{0}' → '{1}'", regex, replacement);
        }
    }
}
