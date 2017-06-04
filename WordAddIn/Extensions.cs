using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GaudiaVedantaPublications
{
    public static class CharacterExtensions
    {
        /// <summary>
        /// For Fonts without Unicode ranges, Word uses Private Use Area for symbols
        /// to avoid collision with other fonts.
        /// http://officeopenxml.com/WPtextSpecialContent-symbol.php
        /// http://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&item_id=PUACharsInMSSotware
        /// http://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=UTTUsingUnicodeMacros
        /// http://www.personal.psu.edu/ejp10/blogs/gotunicode/2008/03/micrsoft-word-logic-inserting.html
        /// https://www.microsoft.com/typography/otspec160/recom.htm
        /// </summary>
        public static char PrivateUseAreaToAnsi(this char c)
        {
            /// If the char is in PUA (0xE000-0xF8FF), then we're taking only least significant byte.
            return Char.GetUnicodeCategory(c) == UnicodeCategory.PrivateUse ? (char)((byte)c) : c;
        }

        public static string PrivateUseAreaToAnsi(this string source)
        {
            /// If no PUA characters, then retrun original string.
            if (!Regex.IsMatch(source, @"\p{IsPrivateUseArea}", RegexOptions.Compiled))
                return source;

            var builder = new StringBuilder(source.Length);
            foreach (var c in source)
                builder.Append(c.PrivateUseAreaToAnsi());

            return builder.ToString();
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
