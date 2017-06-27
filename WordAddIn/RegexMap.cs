using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GaudiaVedantaPublications
{
    public class RegexMap : Map
    {
        protected readonly Regex regex;
        private readonly MatchEvaluator evaluator;

        /// <summary>
        /// Constructor for basic regex replacement.
        /// </summary>
        /// <param name="pattern">Pattern for searching.</param>
        /// <param name="replacement">A string to replace with.</param>
        public RegexMap(string pattern, string replacement)
            : this(pattern, x => replacement)
        {
        }

        /// <summary>
        /// Constructor for complex replacement according to the list or serarch-replacement pairs.
        /// </summary>
        /// <param name="replacements"></param>
        public RegexMap(ILookup<string, string> replacements)
            : this(
                  String.Join("|", replacements.Select(x => Regex.Escape(x.Key))),
                  match => replacements[match.Value].First())
        {
        }

        protected RegexMap(string pattern, MatchEvaluator evaluator)
        {
            this.regex = new Regex(pattern, RegexOptions.Compiled);
            this.evaluator = evaluator;
        }

        public override Map Inverted
        {
            get
            {
                throw new InvalidOperationException("Cannot invert regex-based map");
            }
        }

        public override string Apply(string text)
        {
            return regex.Replace(text, evaluator);
        }

        public override string ToString()
        {
            return String.Format("Regex '{0}'", regex);
        }
    }
}
