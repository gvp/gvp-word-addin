using System;
using System.Linq;
using System.Text;
using StringDictionary = System.Collections.Generic.IDictionary<System.String, System.String>;
using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    class FromUnicodeTransform : IterativeTextTransform
    {
        private readonly String fontName;
        private readonly StringDictionary map;

        public FromUnicodeTransform(String fontName)
        {
            if (fontName == null)
                throw new ArgumentNullException("toFontName");
            this.fontName = fontName;

            map = MapManager.GetMap(fontName);
            if (map != null)
                map = map.ToLookup(x => x.Value, x => x.Key).ToDictionary(x => x.Key, x => x.First());
        }

        public override void Apply(Word.Range range)
        {
            base.Apply(range);
            range.Font.Name = fontName;
        }

        protected override void TransformCharacter(Word.Range character)
        {
            if (character.Font.Name == fontName)
                return;
            character.Text = MapManager.Map(character.Text, map).Normalize(NormalizationForm.FormC);
        }
    }
}
