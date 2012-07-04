using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using StringDictionary = System.Collections.Generic.IDictionary<System.String, System.String>;
using Word = Microsoft.Office.Interop.Word;
using System.Diagnostics;
using System.Text;

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

        public static readonly string[] LatinFonts = { "Rama Garamond Plus", "Balaram", "Amita Times" };
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

        private Word.Words GetWordsToProceed()
        {
            if (Application.Selection.Type == Word.WdSelectionType.wdSelectionNormal)
                return Application.Selection.Words;
            else
                return Application.ActiveDocument.Words;
        }

        public void Transform()
        {
            Application.UndoRecord.StartCustomRecord("Преобразование шрифта");
            Application.ScreenUpdating = false;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var words = GetWordsToProceed();
                for (int n = 1; n <= words.Count; n++)
                {
                    var word = words[n];
                    if (ProcessRange(word))
                        word.Font.Name = toFontName;
                }
            }
            finally
            {
                Trace.WriteLine(stopwatch.Elapsed);
                Application.ScreenUpdating = true;
                Application.UndoRecord.EndCustomRecord();
            }
        }

        private bool ProcessRange(Word.Range range)
        {
            CharacterTransformation transformation;
            //var fontName = range.Font.Name;
            //if (!String.IsNullOrEmpty(fontName))
            //{
            //    if (fontName == toFontName || !transformations.TryGetValue(fontName, out transformation))
            //        return false;

            //    var text = range.Text;
            //    var builder = new StringBuilder(text.Length);
            //    foreach (var character in text)
            //        builder.Append(transformation(character.ToString()));
            //    range.Text = builder.ToString();
            //    return true;
            //}

            var modified = false;
            for (int pos = 1; pos <= range.Characters.Count; pos++)
            {
                var character = range.Characters[pos];
                var fontName = character.Font.Name;
                if (fontName == toFontName || !transformations.TryGetValue(fontName, out transformation))
                    continue;
                character.Text = transformation(character.Text);
                modified = true;
            }
            return modified;
        }

        //private void TransformUsingFind()
        //{
        //    var find = Application.Selection.Find;
        //    find.ClearFormatting();
        //    find.Forward = true;
        //    //find.Wrap = Word.WdFindWrap.wdFindContinue;
        //    if (!String.IsNullOrWhiteSpace(fromFontName))
        //        find.Font.Name = fromFontName;
        //    find.Execute();
        //    while (find.Found)
        //    {
        //        var range = Application.Selection.Range;
        //        var text = range.Text;
        //        var builder = new StringBuilder();
        //        foreach (var character in text)
        //            builder.Append(TransformCharacter(character.ToString()));

        //        text = builder.ToString();
        //        //for (int pos = 1; pos <= range.Characters.Count; pos++)
        //        //{
        //        //    var character = range.Characters[pos];
        //        //    character.Text = TransformCharacter(character.Text);
        //        //}
        //        range.Text = text;
        //        range.Font.Name = toFontName;
        //        find.Execute();
        //    }
        //}

        private static StringDictionary ReadMap(string resourceName)
        {
            var resource = Properties.Resources.ResourceManager.GetString(resourceName);
            if (resource == null)
                return null;
            return XElement.Parse(resource)
                .Elements("entry").Where(x => !x.Attributes("obsolete").Any())
                .ToDictionary(e => e.Attribute("from").Value.Normalize(), e => e.Attribute("to").Value.Normalize());
        }
    }
}
