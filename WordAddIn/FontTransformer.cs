using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using StringDictionary = System.Collections.Generic.IDictionary<System.String, System.String>;
using Word = Microsoft.Office.Interop.Word;

namespace VedicEditor
{
    class FontTransformer
    {
        private readonly String toFontName;
        private readonly StringDictionary toMap;

        public FontTransformer(String toFontName)
        {
            this.toFontName = toFontName ?? Application.ActiveDocument.Styles[Word.WdBuiltinStyle.wdStyleDefaultParagraphFont].Font.Name;

            toMap = ReadMap(toFontName);
            if (toMap != null)
                toMap = toMap.ToLookup(x => x.Value, x => x.Key).ToDictionary(x => x.Key, x => x.First());
        }

        private IEnumerable<StringDictionary> GetMapsFor(String fontName)
        {
            if (fontName != toFontName)
            {
                var fromMap = GetMap(fontName);
                if (fromMap != null)
                    yield return fromMap;

                if (LatinFonts.Contains(fontName))
                    yield return Lat2Cyr;
            }

            if (toMap != null)
                yield return toMap;
        }

        private static readonly StringDictionary Lat2Cyr = ReadMap("Lat2Cyr");

        public static readonly string[] LatinFonts = { "Rama Garamond Plus", "Balaram", "Amita Times", "DVRoman-TTSurekh"
                                                         , "GVPalatino"
                                                         , "ScaBenguit", "ScaCheltenham", "ScaFelixTitling", "ScaFrizQuadrata", "ScaGoudy", "ScaHelvetica", "ScaKorinna", "ScaOptima", "ScaPalatino", "ScaSabon", "ScaTimes"
                                                     };
        public static readonly string[] CyrillicFonts = { "Amita Times Cyr", "ThamesM" };

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
                if (range.Characters.Count == 0)
                    return;
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
            Word.Range character = null;
            for (int pos = 1; pos <= range.Characters.Count; pos++)
            {
                character = range.Characters[pos];
                var fontName = character.Font.Name;
                if (fontName == toFontName)
                    continue;

                var originalText = character.Text;
                var transformedText = originalText.Normalize(NormalizationForm.FormD);

                var fromMap = GetMap(fontName);
                if (fromMap != null)
                    transformedText = Transform(transformedText, fromMap);

                if (CyrillicFonts.Contains(toFontName) && transformedText.First().IsBasicLatin())
                {
                    transformedText = Transform(transformedText, Lat2Cyr);

                    /// Replace Cyrillic 'е' with Cyrillic 'э' in the beginning of the words
                    if (transformedText.First() == '\x0435')
                    {
                        var word = character.Duplicate;
                        word.StartOf(Word.WdUnits.wdWord, Word.WdMovementType.wdExtend);
                        if (word.Start == character.Start)
                            transformedText = '\x044D' + transformedText.Substring(1);
                    }
                }

                transformedText = Transform(transformedText, toMap).Normalize(NormalizationForm.FormC);

                if (transformedText == originalText)
                    continue;

                character.Text = transformedText;
                /// Correct position for cases where transformed text is longer.
                pos += new StringInfo(transformedText).LengthInTextElements - new StringInfo(originalText).LengthInTextElements;
            }

            /// Address an inappropriate behaviour when changing the last character makes range exclude it.
            range.End = character.End;
        }

        private static String Transform(String character, StringDictionary map)
        {
            string result;
            if (map.TryGetValue(character, out result))
                return result;
            return character;
        }

        private readonly IDictionary<String, StringDictionary> maps = new Dictionary<String, StringDictionary>(4);

        private StringDictionary GetMap(String name)
        {
            StringDictionary map;
            if (maps.TryGetValue(name, out map))
                return map;
            map = ReadMap(name);
            maps.Add(name, map);
            return map;
        }

        private static StringDictionary ReadMap(string fontName)
        {
            var resourceName = fontName.StartsWith("Sca") ? "ScaSeries" : fontName;
            var resource = Properties.Resources.ResourceManager.GetString(resourceName);
            if (resource == null)
                return null;
            return XElement.Parse(resource)
                .Elements("entry").Where(x => !x.Attributes("obsolete").Any())
                .ToDictionary(e => e.Attribute("from").Value.Normalize(NormalizationForm.FormD), e => e.Attribute("to").Value.Normalize(NormalizationForm.FormD));
        }
    }
}
