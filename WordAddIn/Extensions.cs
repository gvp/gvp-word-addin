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
    }
}
