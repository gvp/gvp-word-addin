using System.Collections;
using NUnit.Framework;

namespace GaudiaVedantaPublications.Tests
{
    public class FontConversionTests
    {
        public static IEnumerable FontToUnicodeTestCases =>
            FontTestDataProvider.GetFontTestData(FontConversionDirection.FontToUnicode);

        [TestCaseSource(nameof(FontToUnicodeTestCases))]
        public string ToUnicode(string fontName, string ansi) =>
            MappingManager.GetFontToUnicodeMapping(fontName).Apply(ansi);

        public static IEnumerable UnicodeToFontTestCases =>
            FontTestDataProvider.GetFontTestData(FontConversionDirection.UnicodeToFont);

        [TestCaseSource(nameof(UnicodeToFontTestCases))]
        public string FromUnicode(string fontName, string unicode) =>
            MappingManager.GetUnicodeToFontMapping(fontName).Apply(unicode);
    }
}
