using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    class ToUnicodeTransform : MapBasedTextTransform
    {
        public const string UnicodeFontName = "Arial Unicode MS";

        public override void Apply(Word.Range range)
        {
            base.Apply(range);
            range.Font.Name = UnicodeFontName;
        }

        private string currentFontName;
        protected override void TransformCharacter()
        {
            if (CurrentCharacter.Font.Name != currentFontName)
            {
                ApplySavedText();

                currentFontName = CurrentCharacter.Font.Name;
                CurrentMap = MapManager.GetMap(currentFontName, MapDirection.Forward);
            }

            if (CurrentMap == null)
                return;

            base.TransformCharacter();
        }
    }
}
