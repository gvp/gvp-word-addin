using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Mvp.Xml.XInclude;

namespace GaudiaVedantaPublications
{
    /// <summary>
    /// Class that loads the mappings from the resources.
    /// </summary>
    public static class MappingManager
    {
        public static ITextMapping LatinToCyrillic => GetMapping("Latin→Cyrillic");

        public static ITextMapping DevanagariToLatin => GetMapping("Devanagari→Latin");

        public static ITextMapping GetFontToUnicodeMapping(string fontName)
        {
            return GetMapping(GeneralizeFontName(fontName));
        }

        public static ITextMapping GetUnicodeToFontMapping(string fontName)
        {
            return GetMapping(GeneralizeFontName(fontName)).Inverted;
        }

        /// <summary>
        /// Dictionary "Prefix → Generic Font Name"
        /// </summary>
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

        private static readonly MemoryCache cache = new MemoryCache("Mapping Cache");

        /// <summary>
        /// Gets the mapping from cache or load it from resources.
        /// Implementation taken from https://blog.falafel.com/working-system-runtime-caching-memorycache/
        /// </summary>
        /// <param name="name">Mapping name</param>
        /// <returns>Mapping</returns>
        private static ITextMapping GetMapping(string name)
        {
            var newValue = new Lazy<ITextMapping>(() => new NormalizationMapping(LoadMapping(name)));
            var oldValue = cache.AddOrGetExisting(name, newValue, new CacheItemPolicy()) as Lazy<ITextMapping>;
            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch
            {
                // Handle cached lazy exception by evicting from cache. Thanks to Denis Borovnev for pointing this out!
                cache.Remove(name);
                throw;
            }
        }

        private static readonly XmlResolver xmlResolver = new EmbeddedResourcesXmlResolver();

        /// <summary>
        /// Loads mapping from the embedded resource with particular name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static ITextMapping LoadMapping(string name)
        {
            using (var reader = new XIncludingReader(name + ".xml", xmlResolver))
            {
                /// Setting the resolver here according to http://mvp-xml.sourceforge.net/xinclude/#mozTocId795134
                /// Passing as a parameter is also required to resolve the original file
                reader.XmlResolver = xmlResolver;
                return CreateMapping(XElement.Load(reader));
            }
        }

        /// <summary>
        /// Creates mapping from <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static ITextMapping CreateMapping(XElement element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

            if (element.Name == "mapping")
                return new MultiMapping(
                    from entry in element.Elements()
                    orderby (int?)entry.Attribute("order") ?? 0
                    let mapping = CreateMapping(entry)
                    where mapping != null
                    select mapping
                    );

            if (element.Name == "entry")
            {
                if (element.Attribute("from") is null || element.Attribute("to") is null)
                    throw new ArgumentException(string.Format("Entry should have `from` and `to` attributes: {0}", element.ToString()));

                var from = element.Attribute("from").Value.Normalize(System.Text.NormalizationForm.FormC);
                var to = element.Attribute("to").Value.Normalize(System.Text.NormalizationForm.FormC);

                switch (element.Attribute("type")?.Value)
                {
                    case "regex":
                        return new RegexMapping(pattern: from, replacement: to);

                    default:
                        return new ReplaceMapping(from, to);
                }
            }

            throw new ArgumentException(string.Format("Unsupported element name '{0}'", element.Name));
        }

    }
}
