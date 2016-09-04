using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GaudiaVedantaPublications
{
    public class MapEntry
    {
        public string From { get; set; }
        public Regex Regex { get; set; }
        public string To { get; set; }
        public int Order { get; set; }

        public string ReplaceText { get; set; }
        public string ReplaceTextForFirstLetter { get; set; }

        public string InsertBefore { get; set; }
        public string InsertAfter { get; set; }
        public string AppendAfter { get; set; }

        public IDictionary<String, String> Replaces { get; set; }

        public MapEntry()
        {
        }

        public MapEntry(XElement element)
        {
            From = element.AttributeValue("from");
            To = element.AttributeValue("to");
            if (element.AttributeValue("type") == "regex")
                Regex = new Regex(From, RegexOptions.Compiled);

            Order = Int32.Parse(element.AttributeValue("order", "0"));
        }

        public string Apply(string text)
        {
            if (Regex != null)
                return Regex.Replace(text, To);
            else
                return text.Replace(From, To);
        }
    }

    public static partial class Extensions
    {
        public static string Apply(this IOrderedEnumerable<MapEntry> map, string text)
        {
            foreach (var entry in map)
            {
#if TRACE_TEXT_TRANSFORMATION
                var oldText = text;
#endif
                text = entry.Apply(text);
#if TRACE_TEXT_TRANSFORMATION
                if (oldText != text)
                    System.Diagnostics.Trace.TraceInformation("{0} → {1}\twith {3}:{2} → {4} ", oldText, text, entry.From, entry.Order, entry.To);
#endif

            }
            return text;
        }
    }
}
