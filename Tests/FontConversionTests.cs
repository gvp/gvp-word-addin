using NUnit.Framework;

namespace GaudiaVedantaPublications.Tests
{
    public class FontConversionTests
    {
        [TestCaseSource(typeof(FontTestDataProvider), nameof(FontTestDataProvider.GetFontTestData), new object[] { FontConversionDirection.LocalToUnicode })]
        public string ToUnicode(string fontName, string text) =>
            MappingManager.GetFontToUnicodeMapping(fontName).Apply(text);

        [TestCaseSource(typeof(FontTestDataProvider), nameof(FontTestDataProvider.GetFontTestData), new object[] { FontConversionDirection.UnicodeToLocal })]
        public string FromUnicode(string fontName, string text) =>
            MappingManager.GetUnicodeToFontMapping(fontName).Apply(text);
    }
}
