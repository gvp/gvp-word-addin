using System;
using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    class FromUnicodeTransform : MapBasedTextTransform
    {
        private readonly String fontName;

        public FromUnicodeTransform(String fontName)
            : base(MapManager.GetMap(fontName, MapDirection.Backward))
        {
            if (fontName == null)
                throw new ArgumentNullException("toFontName");
            this.fontName = fontName;
        }

        public override void Apply(Word.Range range)
        {
            base.Apply(range);
            range.Font.Name = fontName;
            if (fontName == "ThamesM")
            {
                range.Font.Italic = 1;
                range.Font.Bold = 0;
            }
        }

        protected override void TransformCharacter()
        {
            if (CurrentCharacter.Font.Name == fontName)
                return;

            base.TransformCharacter();
        }
    }
}
