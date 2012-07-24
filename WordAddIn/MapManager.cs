using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using StringDictionary = System.Collections.Generic.IDictionary<System.String, System.String>;

namespace VedicEditor
{
    static class MapManager
    {
        public static String Map(String character, StringDictionary map)
        {
            string result;
            if (map.TryGetValue(character.Normalize(NormalizationForm.FormD), out result))
                return result;
            return character;
        }

        private static readonly IDictionary<String, StringDictionary> maps = new Dictionary<String, StringDictionary>(4);

        public static StringDictionary GetMap(String name)
        {
            StringDictionary map;
            if (maps.TryGetValue(name, out map))
                return map;
            map = ReadMap(name);
            maps.Add(name, map);
            return map;
        }

        private static StringDictionary ReadMap(string fontName)
        {
            var resourceName = fontName.StartsWith("Sca") ? "ScaSeries" : fontName;
            var resource = Properties.Resources.ResourceManager.GetString(resourceName);
            if (resource == null)
                return null;
            return XElement.Parse(resource)
                .Elements("entry").Where(x => !x.Attributes("obsolete").Any())
                .ToDictionary(e => e.Attribute("from").Value.Normalize(NormalizationForm.FormD), e => e.Attribute("to").Value.Normalize(NormalizationForm.FormD));
        }
    }
}
