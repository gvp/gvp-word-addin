using System.Linq;
using System.Text;
using Map = System.Collections.Generic.IDictionary<System.String, GaudiaVedantaPublications.MapEntry>;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    class MapBasedTextTransform : IterativeTextTransform
    {
        public MapBasedTextTransform()
        {
        }

        public MapBasedTextTransform(Map map)
        {
            CurrentMap = map;
        }

        protected Map CurrentMap { get; set; }

        public override void Apply(Word.Range range)
        {
            base.Apply(range);

            if (savedText != null)
            {
                ApplySavedText();
                range.Expand(Word.WdUnits.wdCharacter);
            }
        }

        protected override void TransformCharacter()
        {
            var text = CurrentCharacter.Text.PUAToASCII();

            MapEntry entry;
            if (CurrentMap.TryGetValue(text.Normalize(NormalizationForm.FormD), out entry))
                ApplyEntry(entry);
            else
                CurrentCharacter.Text = text;

            if (savedText == null)
                return;

            savedText.Range.Expand(Word.WdUnits.wdCharacter);

            if (savedText.Range.End <= CurrentCharacter.Start)
                ApplySavedText();
        }

        private void ApplyEntry(MapEntry entry)
        {
            if (entry.ReplaceText != null)
                CurrentCharacter.Text = entry.ReplaceText;

            if (entry.ReplaceTextForFirstLetter != null)
            {
                var word = CurrentCharacter.Duplicate;
                word.StartOf(Word.WdUnits.wdWord, Word.WdMovementType.wdExtend);
                if (word.Start == CurrentCharacter.Start)
                    CurrentCharacter.Text = entry.ReplaceTextForFirstLetter;
            }

            if (entry.InsertBefore != null)
            {
                var previous = CurrentCharacter.Previous();
                previous.InsertBefore(entry.InsertBefore);
            }

            if (entry.InsertAfter != null)
                SaveText(entry.InsertAfter);

            if (entry.AppendAfter != null)
                if (savedText != null)
                    savedText.Text += entry.AppendAfter;
                else
                    CurrentCharacter.Text = entry.AppendAfter;

            if (entry.Replaces.Any())
            {
                CurrentCharacter.MoveStart(Count: -1);
                var text = CurrentCharacter.Text;
                foreach (var replace in entry.Replaces)
                    text = text.Replace(replace.Key, replace.Value);
                CurrentCharacter.Text = text;
            }
        }

        private class SavedText
        {
            public string Text { get; set; }
            public Word.Range Range { get; set; }

            public void Apply()
            {
                Range.Expand(Word.WdUnits.wdCharacter);
                Range.InsertAfter(Text);
            }
        }

        private SavedText savedText;

        protected void SaveText(string text)
        {
            if (savedText != null)
                savedText.Apply();

            savedText = new SavedText
            {
                Text = text,
                Range = CurrentCharacter.Duplicate,
            };
        }

        protected void ApplySavedText()
        {
            if (savedText == null)
                return;

            savedText.Apply();
            savedText = null;
        }
    }
}
