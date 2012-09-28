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
