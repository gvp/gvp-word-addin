using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Map = System.Linq.IOrderedEnumerable<GaudiaVedantaPublications.MapEntry>;

namespace GaudiaVedantaPublications
{
    public enum MapDirection
    {
        Forward,
        Backward,
    }

    public static class MapManager
    {
        private static readonly IDictionary<String, Map> maps = new Dictionary<String, Map>(4);

        public static Map Lat2Cyr
        {
            get
            {
                return GetMap("Lat2Cyr", MapDirection.Forward);
            }
        }

        public static Map Dev2Lat
        {
            get
            {
                return GetMap("Dev2Lat", MapDirection.Forward);
            }
        }

        public static Map GetFontMap(String fontName, MapDirection direction)
        {
            return GetMap(GetMapName(fontName), direction);
        }

        private static string GetMapName(string fontName)
        {
            if (fontName.StartsWith("Sca"))
                return "ScaSeries";
            if (fontName.StartsWith("SDW-"))
                return "SDW";
            if (fontName.StartsWith("KALAKAR"))
                return "KALAKAR";

            return fontName;
        }

        private static readonly string[] devanagariMaps = { "AARitu", "AARituPlus2", "KALAKAR", "SDW" };

        private static Map GetMap(String name, MapDirection direction)
        {
            var key = String.Format("{0}.{1}", name, direction);
            Map map;
            if (maps.TryGetValue(key, out map))
                return map;

            var entries = ReadMap(name);
            if (entries == null)
                return null;

            /// For devanagari fonts there is a common section
            if (devanagariMaps.Contains(name))
                entries = entries.Concat(ReadMap("Devanagari2Unicode")); //TODO: take direction into account

            map = entries.OrderBy(e => e.Order);
            maps.Add(key, map);
            return map;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>null means that there is no map for this font</returns>
        private static IEnumerable<MapEntry> ReadMap(string name)
        {
            using (var resource = EmbeddedResourceManager.GetEmbeddedResource(name + ".xml"))
            {
                if (resource == null)
                    return null;

                return
                    from element in XElement.Load(resource).Elements("entry")
                    select new MapEntry(element);
            }
        }
    }
}
