using NUnit.Framework;

namespace GaudiaVedantaPublications.Tests
{
    [TestOf(typeof(MappingManager))]
    [Category("Mapping Manager")]
    public class MappingManagerTests
    {
        [TestCase("ScaPalatino", "ScaGauda")]
        [TestCase("AARituPlus2", "AARituPlus2")]
        [TestCase("ThamesM", "ThamesM")]
        [TestCase("Amita Times Cyr", "Krishna Times Plus")]
        public void ShouldReturnSameMapping(string name1, string name2) =>
            Assert.AreSame(MappingManager.GetFontToUnicodeMapping(name1), MappingManager.GetFontToUnicodeMapping(name2));

        [TestCase("Another")]
        [TestCase("NonSupported")]
        [TestCase("Arial Unicode MS")]
        public void ShouldReturnNullForNonSupportedFont(string name) =>
            Assert.IsNull(MappingManager.GetFontToUnicodeMapping(name));

        [TestCaseSource(typeof(FontTestDataProvider), nameof(FontTestDataProvider.GetFontNamesForConversion), new object[] { FontConversionDirection.FontToUnicode })]
        public void ShouldReturnFontToUnicodeMapping(string fontName) =>
            Assert.NotNull(MappingManager.GetFontToUnicodeMapping(fontName));

        [TestCaseSource(typeof(FontTestDataProvider), nameof(FontTestDataProvider.GetFontNamesForConversion), new object[] { FontConversionDirection.UnicodeToFont })]
        public void ShouldReturnUnicodeToFontMapping(string fontName) =>
            Assert.NotNull(MappingManager.GetUnicodeToFontMapping(fontName));

        [Test]
        public void ShouldReturnTranslitMappings()
        {
            Assert.NotNull(MappingManager.DevanagariToLatin);
            Assert.NotNull(MappingManager.LatinToCyrillic);
        }
    }
}
