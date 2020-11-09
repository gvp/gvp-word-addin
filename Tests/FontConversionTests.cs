using NUnit.Framework;

namespace GaudiaVedantaPublications.Tests
{
    public class FontConversionTests
    {
        [TestCaseSource(typeof(FontTestDataProvider), nameof(FontTestDataProvider.GetFontTestData), new object[] { FontConversionDirection.FontToUnicode })]
        public string ToUnicode(string fontName, string ansi) =>
            MappingManager.GetFontToUnicodeMapping(fontName).Apply(ansi);

        [TestCaseSource(typeof(FontTestDataProvider), nameof(FontTestDataProvider.GetFontTestData), new object[] { FontConversionDirection.UnicodeToFont })]
        public string FromUnicode(string fontName, string unicode) =>
            MappingManager.GetUnicodeToFontMapping(fontName).Apply(unicode);
    }
}
