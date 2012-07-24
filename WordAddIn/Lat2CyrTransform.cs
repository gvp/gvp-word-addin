using System.Linq;
using System.Text;
using StringDictionary = System.Collections.Generic.IDictionary<System.String, System.String>;
using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    class Lat2CyrTransform : IterativeTextTransform
    {
        private static readonly StringDictionary map = MapManager.GetMap("Lat2Cyr");

        protected override void TransformCharacter(Word.Range character)
        {
            var text = character.Text.Normalize(NormalizationForm.FormD);

            if (!text.First().IsBasicLatin())
                return;
            text = MapManager.Map(text, map);

            /// Replace Cyrillic 'е' with Cyrillic 'э' in the beginning of the words
            if (text.First() == '\x0435')
            {
                var word = character.Duplicate;
                word.StartOf(Word.WdUnits.wdWord, Word.WdMovementType.wdExtend);
                if (word.Start == character.Start)
                    text = '\x044D' + text.Substring(1);
            }
            character.Text = text;
        }
    }
}
