using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    public static partial class Extensions
    {
        public static bool IsSame(this Word.Font font, Word.Font other)
        {
            return other.Name == font.Name
                && other.Size == font.Size
                && other.Color == font.Color
                && other.Bold == font.Bold
                && other.Italic == font.Italic
                && other.Underline == font.Underline
                && other.UnderlineColor == font.UnderlineColor
                && other.StrikeThrough == font.StrikeThrough
                ;
        }
    }
}
