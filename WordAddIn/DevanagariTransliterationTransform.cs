using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map = System.Collections.Generic.IDictionary<System.String, VedicEditor.MapEntry>;

namespace VedicEditor
{
    class DevanagariTransliterationTransform : IterativeTextTransform
    {
        private static Map map = MapManager.Dev2Lat;

        protected override void TransformCharacter()
        {
            var text = CurrentCharacter.Text;
            var builder = new StringBuilder();
            foreach (var c in text)
            {
                MapEntry entry;
                if (!map.TryGetValue(c.ToString(), out entry))
                    entry.ReplaceText = c.ToString();

                switch (GetAlphabetCategory(c))
                {
                    case AlphabetCategory.Consonant:
                        builder.Append(entry.ReplaceText);
                        builder.Append("a");
                        break;

                    case AlphabetCategory.VowelSign:
                        builder.Remove(builder.Length - 1, 1);
                        builder.Append(entry.ReplaceText);
                        break;

                    case AlphabetCategory.Unknown:
                        builder.Append(c);
                        break;

                    default:
                        builder.Append(entry.ReplaceText);
                        break;
                }
            }
            CurrentCharacter.Text = builder.ToString();
        }

        private enum AlphabetCategory
        {
            Unknown,
            Sign,
            Consonant,
            Vowel,
            VowelSign,
            Other,
        }

        private static IDictionary<char, AlphabetCategory> CategoryNodes = new Dictionary<char, AlphabetCategory>
        {
            { '\x0000', AlphabetCategory.Unknown },
            { '\x0900', AlphabetCategory.Sign },
            { '\x0904', AlphabetCategory.Vowel },
            { '\x0915', AlphabetCategory.Consonant },
            { '\x093A', AlphabetCategory.VowelSign },
            { '\x093C', AlphabetCategory.Other },
            { '\x093E', AlphabetCategory.VowelSign },
            { '\x0950', AlphabetCategory.Other },
            { '\x0951', AlphabetCategory.Sign },
            { '\x0955', AlphabetCategory.VowelSign },
            { '\x0958', AlphabetCategory.Consonant },
            { '\x0960', AlphabetCategory.Vowel },
            { '\x0962', AlphabetCategory.VowelSign },
            { '\x0964', AlphabetCategory.Other },
            { '\x0980', AlphabetCategory.Unknown },
        };

        private static AlphabetCategory GetAlphabetCategory(char character)
        {
            var codepoint = CategoryNodes.Keys.Where(k => k <= character).Max();
            return CategoryNodes[codepoint];
        }
    }
}
