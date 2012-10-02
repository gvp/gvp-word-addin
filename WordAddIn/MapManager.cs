using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Map = System.Collections.Generic.IDictionary<System.String, GaudiaVedantaPublications.MapEntry>;

namespace GaudiaVedantaPublications
{
    public enum MapDirection
    {
        Forward,
        Backward,
    }

    static class MapManager
    {
        private static readonly IDictionary<String, Map> maps = new Dictionary<String, Map>(4);

        public static Map Lat2Cyr
        {
            get
            {
                return GetMap("Translit.Lat2Cyr", MapDirection.Forward);
            }
        }

        public static Map Dev2Lat
        {
            get
            {
                return GetMap("Translit.Dev2Lat", MapDirection.Forward);
            }
        }

        public static Map GetFontMap(String fontName, MapDirection direction)
        {
            if (fontName.StartsWith("Sca"))
                fontName = "ScaSeries";
            return GetMap("Fonts." + fontName, direction);
        }

        private static Map GetMap(String name, MapDirection direction)
        {
            var key = String.Format("{0}.{1}", name, direction);
            Map map;
            if (maps.TryGetValue(key, out map))
                return map;
            map = ReadMap(name, direction);
            maps.Add(key, map);
            return map;
        }

        private static Map ReadMap(string name, MapDirection direction)
        {
            var resourceName = String.Format("Maps.{0}.xml", name);
            using (var resource = EmbeddedResourceManager.GetEmbeddedResource(resourceName))
            {
                if (resource == null)
                    return null;
                return ReadMap(resource, direction);
            }
        }

        private static Map ReadMap(Stream resource, MapDirection direction)
        {
            return (
                from element in XElement.Load(resource).Elements("entry")
                let entryDirection = element.AttributeValue("direction", element.Elements().Any() ? "forward" : null)
                where entryDirection == null || entryDirection == direction.ToString().ToLower()
                let reverse = entryDirection == null && direction == MapDirection.Backward
                select new
                {
                    Key = element.Attribute(reverse ? "to" : "from").Value.Normalize(NormalizationForm.FormD),
                    Entry = new MapEntry
                    {
                        ReplaceText = element.AttributeValue(reverse ? "from" : "to"),
                        ReplaceTextForFirstLetter = element.ElementValue("firstLetter"),
                        InsertBefore = element.ElementValue("insertBefore"),
                        InsertAfter = element.ElementValue("insertAfter"),
                        AppendAfter = element.ElementValue("appendAfter"),
                        Replaces = element.Elements("replace").ToDictionary(r => r.AttributeValue("from"), r => r.AttributeValue("to")),
                    },
                }).ToDictionary(e => e.Key, e => e.Entry);
        }
    }
}
