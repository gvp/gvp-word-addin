using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GaudiaVedantaPublications
{
    public static class CharacterExtensions
    {
        public static bool IsBasicLatin(this char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        /// <summary>
        /// For Fonts without Unicode ranges, Word uses Private Use Area for symbols
        /// to avoid collision with other fonts.
        /// http://officeopenxml.com/WPtextSpecialContent-symbol.php
        /// http://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&item_id=PUACharsInMSSotware
        /// http://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=UTTUsingUnicodeMacros
        /// http://www.personal.psu.edu/ejp10/blogs/gotunicode/2008/03/micrsoft-word-logic-inserting.html
        /// https://www.microsoft.com/typography/otspec160/recom.htm
        /// </summary>
        public static string PUAToASCII(this string source)
        {
            return new string(
                (
                from c in source
                select '\xF000' <= c && c <= '\xF0FF' ? (char)(c - '\xF000') : c
                ).ToArray()
                );
        }

        public static String AttributeValue(this XElement element, XName name)
        {
            return element.AttributeValue(name, null);
        }

        public static String AttributeValue(this XElement element, XName name, String defaultValue)
        {
            var attribute = element.Attribute(name);
            if (attribute == null)
                return defaultValue;
            return attribute.Value;
        }

        public static String ElementValue(this XElement element, XName name)
        {
            var subElement = element.Element(name);
            if (subElement == null)
                return null;
            return subElement.Value;
        }
    }
}
