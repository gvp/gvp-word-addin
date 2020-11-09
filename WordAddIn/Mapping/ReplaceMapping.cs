namespace GaudiaVedantaPublications
{
    /// <summary>
    /// Mapping that uses <see cref="string.Replace"/> method.
    /// </summary>
    public class ReplaceMapping : ITextMapping
    {
        private readonly string from;
        private readonly string to;

        public ReplaceMapping(string from, string to)
        {
            this.from = from;
            this.to = to;
        }

        public ITextMapping Inverted => new ReplaceMapping(to, from);

        public string Apply(string text)
        {
            var oldText = text;
            text = text.Replace(from, to);
            if (oldText != text)
                System.Diagnostics.Trace.TraceInformation("'{0}' → '{1}'\twith {2}", oldText, text, this);
            return text;
        }

        public override string ToString()
        {
            return string.Format("'{0}' → '{1}'", from, to);
        }
    }
}
