using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    class ToUnicodeTransform : MapBasedTextTransform
    {
        public const string UnicodeFontName = "Calibri";
        
        public override void Apply(Word.Range range)
        {
            base.Apply(range);
            range.Font.Name = UnicodeFontName;
            range.Font.NameBi = UnicodeFontName;
            range.Font.NameAscii = UnicodeFontName;
            range.Font.NameOther = UnicodeFontName;
        }

        private string currentFontName;
        protected override void TransformCharacter()
        {
            if (CurrentCharacter.Font.Name != currentFontName)
            {
                ApplySavedText();

                currentFontName = CurrentCharacter.Font.Name;
                CurrentMap = MapManager.GetFontMap(currentFontName, MapDirection.Forward);
            }

            if (CurrentMap == null)
                return;

            base.TransformCharacter();
        }
    }
}
