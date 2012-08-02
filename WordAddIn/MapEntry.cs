using System;
using System.Collections.Generic;

namespace VedicEditor
{
    struct MapEntry
    {
        public string ReplaceText { get; set; }
        public string ReplaceTextForFirstLetter { get; set; }

        public string InsertBefore { get; set; }
        public string InsertAfter { get; set; }
        public string AppendAfter { get; set; }

        public IDictionary<String, String> Replaces { get; set; }
    }
}
