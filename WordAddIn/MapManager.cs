using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Map = System.Linq.IOrderedEnumerable<GaudiaVedantaPublications.MapEntry>;

namespace GaudiaVedantaPublications
{
    public static class MapManager
    {
        private static readonly IDictionary<String, Map> maps = new Dictionary<String, Map>(4);
        private const string Unicode = "Unicode";
        private const string Devanagari = "Devanagari";
        private const string Cyrillic = "Cyrillic";
        private const string Latin = "Latin";

        public static Map LatinToCyrillic
        {
            get
            {
                return GetMap(Latin, Cyrillic);
            }
        }

        public static Map DevanagariToLatin
        {
            get
            {
                return GetMap(Devanagari, Latin);
            }
        }

        public static Map GetFontToUnicodeMap(String fontName)
        {
            return GetMap(GetMapName(fontName), Unicode);
        }

        public static Map GetUnicodeToFontMap(String fontName)
        {
            return GetMap(Unicode, GetMapName(fontName));
        }

        private static readonly IDictionary<string, string> FontEquivalence = new Dictionary<string, string>
        {
            { "Sca", "ScaSeries" },
            { "SDW-", "SDW" },
            { "KALAKAR", "KALAKAR" },
            { "Krishna Times Plus", "Amita Times Cyr" },
        };
        private static string GetMapName(string fontName)
        {
            return (
                from entry in FontEquivalence
                where fontName.StartsWith(entry.Key)
                select entry.Value
                ).DefaultIfEmpty(fontName).First();
        }

        public static readonly string[] DevanagariFonts = { "AARitu", "AARituPlus2", "AARituPlus2-Numbers", "AAVishal", "KALAKAR", "SDW" };

        public static Map GetMap(string source, string destination)
        {
            var key = String.Format("{0}→{1}", source, destination);
            Map map;
            if (maps.TryGetValue(key, out map))
                return map;

            var entries = ReadMap(source, destination);
            if (entries == null)
                return null;

            /// For devanagari fonts there is a common section
            if (DevanagariFonts.Contains(source))
                entries = entries.Concat(ReadMap(Devanagari, Unicode));
            else if (DevanagariFonts.Contains(destination))
                entries = entries.Concat(ReadMap(Unicode, Devanagari));

            map = entries
                .OrderBy(e => e.Order);
            maps.Add(key, map);
            return map;
        }

        /// <summary>
        /// Attempts to read the map in different ways: bidirectional or uni-directional.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns>null means that there is no resource for this combination</returns>
        private static IEnumerable<MapEntry> ReadMap(string source, string destination)
        {
            /// First, looking for "source→destination.xml"
            var result = ReadMap(String.Format("{0}→{1}", source, destination));
            if (result != null)
                return result;

            /// Falling back to bidirectional maps for Unicode transformations.
            if (destination == Unicode)
                return ReadMap(source);

            if (source == Unicode)
                return ReadMap(destination).Select(e => e.Inverted);

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>null means that there is no resource of this name</returns>
        private static IEnumerable<MapEntry> ReadMap(string name)
        {
            using (var resource = EmbeddedResourceManager.GetEmbeddedResource(name + ".xml"))
            {
                if (resource == null)
                    return null;

                return
                    from element in XElement.Load(resource).Elements("entry")
                    select MapEntry.Create(element);
            }
        }
    }
}
