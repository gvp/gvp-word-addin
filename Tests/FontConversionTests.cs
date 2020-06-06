using System.Text;
using Xunit;

namespace GaudiaVedantaPublications.Tests
{
    [Trait("Category", "Font Conversion")]
    public class FontConversionTests
    {
        [Theory]
        [Trait("Category", "To Unicode")]
        [MemberData(nameof(FontTestDataProvider.GetFontTestData), FontConversionDirection.FontToUnicode, MemberType = typeof(FontTestDataProvider))]
        public void ToUnicode(string unicode, string ansi, string fontName)
        {
            var map = MappingManager.GetFontToUnicodeMapping(fontName);
            Assert.Equal(unicode.Normalize(NormalizationForm.FormC), map.Apply(ansi));
        }

        [Theory]
        [Trait("Category", "From Unicode")]
        [MemberData(nameof(FontTestDataProvider.GetFontTestData), FontConversionDirection.UnicodeToFont, MemberType = typeof(FontTestDataProvider))]
        public void FromUnicode(string unicode, string ansi, string fontName)
        {
            var map = MappingManager.GetUnicodeToFontMapping(fontName);
            Assert.Equal(ansi.Normalize(NormalizationForm.FormC), map.Apply(unicode));
        }
    }
}
