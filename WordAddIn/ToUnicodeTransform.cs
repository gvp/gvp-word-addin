using System.Text;
using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    class ToUnicodeTransform : IterativeTextTransform
    {
        public const string UnicodeFontName = "Arial Unicode MS";

        public override void Apply(Word.Range range)
        {
            base.Apply(range);
            range.Font.Name = UnicodeFontName;
        }

        protected override void TransformCharacter(Word.Range character)
        {
            var map = MapManager.GetMap(character.Font.Name);
            if (map == null)
                return;

            character.Text = MapManager.Map(character.Text, map).Normalize(NormalizationForm.FormC);
        }
    }
}
