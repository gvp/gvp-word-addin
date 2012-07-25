using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VedicEditor
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
    }
}
