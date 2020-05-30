using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;

namespace GaudiaVedantaPublications
{
    /// <summary>
    /// Class that loads the mappings from the resources.
    /// </summary>
    public static class MappingManager
    {
        private const string Unicode = "Unicode";
        private const string Devanagari = "Devanagari";
        private const string Cyrillic = "Cyrillic";
        private const string Latin = "Latin";

        public static ITextMapping LatinToCyrillic
        {
            get
            {
                return GetMapping(Latin, Cyrillic);
            }
        }

        public static ITextMapping DevanagariToLatin
        {
            get
            {
                return GetMapping(Devanagari, Latin);
            }
        }

        public static ITextMapping GetFontToUnicodeMapping(String fontName)
        {
            return GetMapping(GeneralizeFontName(fontName), Unicode);
        }

        public static ITextMapping GetUnicodeToFontMapping(String fontName)
        {
            return GetMapping(Unicode, GeneralizeFontName(fontName));
        }

        private static readonly IDictionary<string, string> FontEquivalence = new Dictionary<string, string>
        {
            { "Sca", "ScaSeries" },
            { "SDW-", "SDW" },
            { "KALAKAR", "KALAKAR" },
            { "Krishna Times Plus", "Amita Times Cyr" },
        };
        private static string GeneralizeFontName(string fontName)
        {
            return (
                from entry in FontEquivalence
                where fontName.StartsWith(entry.Key)
                select entry.Value
                ).DefaultIfEmpty(fontName).First();
        }

        public static readonly string[] DevanagariFonts = { "AARitu", "AARituPlus2", "AARituPlus2-Numbers", "AAVishal", "KALAKAR", "SDW" };

        public static ITextMapping GetMapping(string source, string destination)
        {
            var mapping = LoadMapping(source, destination);
            if (mapping == null)
                return null;
            var key = String.Format("{0}→{1}", source, destination);
            return AddOrGetExisting(key, () => new NormalizationMapping(mapping));
        }


        #region Cache
        private static readonly MemoryCache cache = new MemoryCache("Mapping Cache");

        /// <summary>
        /// Implementation taken from https://blog.falafel.com/working-system-runtime-caching-memorycache/
        /// </summary>
        private static ITextMapping AddOrGetExisting(string key, Func<ITextMapping> factory)
        {
            var newValue = new Lazy<ITextMapping>(factory);
            var oldValue = cache.AddOrGetExisting(key, newValue, new CacheItemPolicy()) as Lazy<ITextMapping>;
            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                // Handle cached lazy exception by evicting from cache. Thanks to Denis Borovnev for pointing this out!
                cache.Remove(key);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// Loads mapping suitable for converting from the <paramref name="source"/> representation
        /// into the <paramref name="destination"/> representation.
        /// First it looks for the unidirectional mapping <paramref name="source"/>→<paramref name="destination"/>,
        /// then resorts to loading a bidirectional mapping for either <paramref name="destination"/> (direct)
        /// or <paramref name="source"/> (inverted).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns>null means that there is no resource for this combination</returns>
        private static ITextMapping LoadMapping(string source, string destination)
        {
            /// First, looking for "source→destination"
            var result = LoadMapping(String.Format("{0}→{1}", source, destination));
            if (result != null)
                return result;

            /// Falling back to bidirectional maps for Unicode transformations.
            if (destination == Unicode)
                return LoadMapping(source);

            if (source == Unicode)
                return LoadMapping(destination).Inverted;

            return null;
        }

        /// <summary>
        /// Loads mapping from the embedded resource with particular name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static ITextMapping LoadMapping(string name)
        {
            return LoadMapping(LoadMappingObject(name));
        }

        /// <summary>
        /// Loads mapping from <see cref="JToken"/>.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private static ITextMapping LoadMapping(JToken root)
        {
            if (root == null)
                return null;

            if (root["include"] != null)
                return LoadMapping((string)root["include"]);

            if (root["entries"] != null)
                return LoadMultiMapping(root);

            if (root["from"] != null)
                return LoadMappingEntry(root);

            return null;
        }

        private static ITextMapping LoadMappingEntry(JToken root)
        {
            switch ((string)root["type"])
            {
                case "regex":
                    return new RegexMapping(
                        pattern: (string)root["from"],
                        replacement: (string)root["to"]
                        );

                default:
                    return new ReplaceMapping(
                        from: (string)root["from"],
                        to: (string)root["to"]
                        );
            }
        }

        private static ITextMapping LoadMultiMapping(JToken root)
        {
            return new MultiMapping(
                from entry in root["entries"]
                orderby (int)(entry["order"] ?? 0)
                let map = LoadMapping(entry)
                where map != null
                select map
                );
        }

        /// <summary>
        /// Loads embedded JSON resource into a <see cref="JObject"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>null means that there is no resource of this name</returns>
        private static JObject LoadMappingObject(string name)
        {
            using (var resource = EmbeddedResourceManager.GetEmbeddedResource(name + ".json"))
            {
                if (resource == null)
                    return null;

                using (var reader = new StreamReader(resource))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return JObject.Load(jsonReader);
                }
            }
        }
    }
}
