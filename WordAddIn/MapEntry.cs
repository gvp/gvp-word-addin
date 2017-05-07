using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GaudiaVedantaPublications
{
    public abstract class MapEntry
    {
        public int Order { get; set; }

        protected MapEntry(int order)
        {
            this.Order = order;
        }

        public static MapEntry Create(XElement element)
        {
            var from = element.AttributeValue("from").Normalize(System.Text.NormalizationForm.FormC);
            var to = element.AttributeValue("to").Normalize(System.Text.NormalizationForm.FormC);
            var order = Int32.Parse(element.AttributeValue("order", "0"));

            switch (element.AttributeValue("type"))
            {
                case "regex":
                    return new RegexMapEntry(order, from, to);

                default:
                    return new SimpleMapEntry(order, from, to);
            }
        }

        public abstract string Apply(string text);

        public abstract MapEntry Inverted
        {
            get;
        }
    }

    public class SimpleMapEntry : MapEntry
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

        public SimpleMapEntry(int order, string from, string to)
            : base(order)
        {
            this.from = from;
            this.to = to;
        }

        public override MapEntry Inverted
        {
            get
            {
                return new SimpleMapEntry(-Order, to, from);
            }
        }

        public override string Apply(string text)
        {
            return text.Replace(from, to);
        }

        public override string ToString()
        {
            return String.Format("'{0}' →'{1}'", from, to);
        }
    }

    public class RegexMapEntry : MapEntry
    {
        protected readonly Regex regex;
        private readonly string replacement;

        public RegexMapEntry(int order, string pattern, string replacement)
            : base(order)
        {
            this.regex = new Regex(pattern, RegexOptions.Compiled);
            this.replacement = replacement;
        }

        public override MapEntry Inverted
        {
            get
            {
                throw new InvalidOperationException("Cannot invert regex-based map entry");
            }
        }

        public override string Apply(string text)
        {
            return regex.Replace(text, replacement);
        }

        public override string ToString()
        {
            return String.Format("'{0}' →'{1}'", regex, replacement);
        }
    }

    public class MultiReplaceMapEntry : MapEntry
    {
        private readonly ILookup<string, string> replacements;
        private readonly Regex regex;

        public MultiReplaceMapEntry(int order, ILookup<string, string> replacements)
            : base(order)
        {
            this.replacements = replacements;
            var keys = from item in replacements
                       select Regex.Escape(item.Key);

            this.regex = new Regex(String.Join("|", keys), RegexOptions.Compiled);
        }

        public override MapEntry Inverted
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Apply(string text)
        {
            return regex.Replace(text, new MatchEvaluator(match => replacements[match.Value].First()));
        }

        public override string ToString()
        {
            return String.Format("'{0}' → multi", regex);
        }
    }

    public static partial class Extensions
    {
        public static string Apply(this IOrderedEnumerable<MapEntry> map, string text)
        {
            text = text.Normalize(System.Text.NormalizationForm.FormC);
            foreach (var entry in map)
            {
#if TRACE_TEXT_TRANSFORMATION
                var oldText = text;
#endif
                text = entry.Apply(text);
#if TRACE_TEXT_TRANSFORMATION
                if (oldText != text)
                    System.Diagnostics.Trace.TraceInformation("'{0}' → '{1}'\twith {2}: {3}", oldText, text, entry.Order, entry);
#endif
            }
            return text.Normalize(System.Text.NormalizationForm.FormC);
        }

        public static IEnumerable<MapEntry> CombineSimpleEntries(this IEnumerable<MapEntry> map)
        {
            var groups =
                from entry in map
                group entry by new { Order = entry.Order, IsSimple = entry is SimpleMapEntry };

            var combined =
                from g in groups
                where g.Key.IsSimple
                select new MultiReplaceMapEntry(g.Key.Order, g.OfType<SimpleMapEntry>().ToLookup(x => x.From, x => x.To));

            var others =
                from g in groups
                where !g.Key.IsSimple
                from entry in g
                select entry;

            return combined.Union(others);
        }
    }
}
