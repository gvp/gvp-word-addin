using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GaudiaVedantaPublications
{




    public static partial class Extensions
    {
//        public static string Apply(this IOrderedEnumerable<MapEntry> map, string text)
//        {
//            text = text.Normalize(System.Text.NormalizationForm.FormC).PrivateUseAreaToAnsi();
//            foreach (var entry in map)
//            {
//#if TRACE_TEXT_TRANSFORMATION
//                var oldText = text;
//#endif
//                text = entry.Apply(text);
//#if TRACE_TEXT_TRANSFORMATION
//                if (oldText != text)
//                    System.Diagnostics.Trace.TraceInformation("'{0}' → '{1}'\twith {2}: {3}", oldText, text, entry.Order, entry);
//#endif
//            }
//            return text.Normalize(System.Text.NormalizationForm.FormC);
//        }

        //public static IEnumerable<MapEntry> CombineSimpleEntries(this IEnumerable<MapEntry> map)
        //{
        //    var groups =
        //        from entry in map
        //        group entry by new { Order = entry.Order, IsSimple = entry is SimpleMapEntry };

        //    var combined =
        //        from g in groups
        //        where g.Key.IsSimple
        //        select new MultiReplaceMapEntry(g.Key.Order, g.OfType<SimpleMapEntry>().ToLookup(x => x.From, x => x.To));

        //    var others =
        //        from g in groups
        //        where !g.Key.IsSimple
        //        from entry in g
        //        select entry;

        //    return combined.Union(others);
        //}
    }
}
