using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using StringDictionary = System.Collections.Generic.IDictionary<System.String, System.String>;
using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    class FontTransformer
    {
        private delegate String CharacterTransformation(String character);

        private readonly IDictionary<String, CharacterTransformation> transformations = new Dictionary<String, CharacterTransformation>(4);

        private readonly String toFontName;

        public FontTransformer(String toFontName)
        {
            this.toFontName = toFontName ?? Application.ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleDefaultParagraphFont].Font.Name;

            var toMap = ReadMap(toFontName);
            if (toMap != null)
                toMap = toMap.ToLookup(x => x.Value, x => x.Key).ToDictionary(x => x.Key, x => x.First());

            var lat2cyr = ReadMap("Lat2Cyr");
            foreach (var fontName in LatinFonts)
                transformations.Add(fontName, GetTransformation(ReadMap(fontName), lat2cyr, toMap));

            foreach (var fontName in CyrillicFonts)
                transformations.Add(fontName, GetTransformation(ReadMap(fontName), toMap));
        }

        private static CharacterTransformation GetTransformation(params StringDictionary[] maps)
        {
            maps = maps.Where(m => m != null).ToArray();
            return c =>
                {
                    string value;

                    foreach (var map in maps)
                        if (map.TryGetValue(c, out value))
                            c = value;

                    return c;
                };
        }

        public static readonly string[] LatinFonts = { "Rama Garamond Plus", "Balaram", "Amita Times", "DVRoman-TTSurekh"
                                                         , "ScaBenguit", "ScaCheltenham", "ScaFelixTitling", "ScaFrizQuadrata", "ScaGoudy", "ScaHelvetica", "ScaKorinna", "ScaOptima", "ScaPalatino", "ScaSabon", "ScaTimes"
                                                     };
        public static readonly string[] CyrillicFonts = { "Amita Times Cyr", "ThamesM" };

        public static IEnumerable<String> Fonts
        {
            get
            {
                return LatinFonts.Union(CyrillicFonts);
            }
        }

        private Word.Application Application
        {
            get
            {
                return Globals.ThisAddIn.Application;
            }

        }

        public void Transform()
        {
            Application.UndoRecord.StartCustomRecord("Преобразование шрифта");
            Application.ScreenUpdating = false;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var range = Application.Selection.Range;
                Transform(range);
                range.Font.Name = toFontName;
                range.Select();
            }
            finally
            {
                Trace.WriteLine(stopwatch.Elapsed);
                Application.ScreenUpdating = true;
                Application.UndoRecord.EndCustomRecord();
            }
        }

        private void Transform(Word.Range range)
        {
            var fontName = range.Font.Name;

            /// Test if the whole range has the only font name.
            if (String.IsNullOrEmpty(fontName))
            {
                var subranges = range.GetSubRanges();
                foreach (var subrange in subranges)
                    Transform(subrange);
                return;
            }

            CharacterTransformation transformation;
            if (fontName == toFontName || !transformations.TryGetValue(fontName, out transformation))
                return;

            for (int pos = 1; pos <= range.Characters.Count; pos++)
            {
                var character = range.Characters[pos];
                var originalText = character.Text;
                var transformedText = transformation(originalText);
                if (transformedText == originalText)
                    continue;

                character.Text = transformedText;
                /// Correct position for cases where transformed text is longer.
                pos += transformedText.Length - originalText.Length;
            }
        }

        private static StringDictionary ReadMap(string fontName)
        {
            var resourceName = fontName.StartsWith("Sca") ? "ScaSeries" : fontName;
            var resource = Properties.Resources.ResourceManager.GetString(resourceName);
            if (resource == null)
                return null;
            return XElement.Parse(resource)
                .Elements("entry").Where(x => !x.Attributes("obsolete").Any())
                .ToDictionary(e => e.Attribute("from").Value.Normalize(), e => e.Attribute("to").Value.Normalize());
        }
    }

    public static class WordExtensions
    {
        public static IEnumerable<Word.Range> GetSubRanges(this Word.Range range)
        {
            if (range.Paragraphs.Count > 1)
                return
                    from Word.Paragraph paragraph in range.Paragraphs
                    select paragraph.Range;

            if (range.Sentences.Count > 1)
                return range.Sentences.OfType<Word.Range>();

            if (range.Words.Count > 1)
                return range.Words.OfType<Word.Range>();

            return range.Characters.OfType<Word.Range>();
        }
    }

}
