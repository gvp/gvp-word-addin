using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    public class RemoveDiacryticsTransform : ITextTransform
    {
        public void Apply(Range range)
        {
            range.Text = Regex.Replace(range.Text, @"\p{IsCombiningDiacriticalMarks}", String.Empty);
        }
    }
}
