using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    class ToUnicodeTransform : MapBasedTextTransform
    {
        /// <summary>
        /// Only this font supports most unicode blocks. And it is required to render Devanagari correctly.
        /// </summary>
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
                CurrentMap = MapManager.GetFontMap(currentFontName, MapDirection.Forward);
            }

            if (CurrentMap == null)
                return;

            base.TransformCharacter();
        }
    }
}
