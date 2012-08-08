using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    class ToUnicodeTransform : MapBasedTextTransform
    {
        public override void Apply(Word.Range range)
        {
            base.Apply(range);
            var normalFont = range.Document.Styles[Word.WdBuiltinStyle.wdStyleNormal].Font;
            range.Font.Name = normalFont.Name;
            range.Font.NameBi = normalFont.NameBi;
            range.Font.NameAscii = normalFont.NameAscii;
            range.Font.NameOther = normalFont.NameOther;
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
