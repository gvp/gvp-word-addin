﻿using System;
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
            regex = new Regex(pattern, RegexOptions.Compiled);
            this.replacement = replacement;
        }

        public ITextMapping Inverted => throw new InvalidOperationException("Cannot invert regex-based map");

        public string Apply(string text)
        {
            var oldText = text;
            text = regex.Replace(text, replacement);
            if (oldText != text)
                System.Diagnostics.Trace.TraceInformation("'{0}' → '{1}'\twith {2}", oldText, text, this);
            return text;
        }

        public override string ToString()
        {
            return string.Format("'{0}' → '{1}'", regex, replacement);
        }
    }
}
