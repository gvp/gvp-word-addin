using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GaudiaVedantaPublications.Tests
{
    [Trait("Category", "Mapping")]
    public class MappingManagerTests
    {
        [Theory]
        [InlineData("ScaPalatino", "ScaGauda")]
        [InlineData("SDW", "SDW-Surekh")]
        [InlineData("KALAKAR", "KALAKAR1")]
        [InlineData("AARituPlus2", "AARituPlus2")]
        [InlineData("ThamesM", "ThamesM")]
        [InlineData("Amita Times Cyr", "Krishna Times Plus")]
        public void ShouldReturnSameMapping(string name1, string name2)
        {
            Assert.Same(MappingManager.GetFontToUnicodeMapping(name1), MappingManager.GetFontToUnicodeMapping(name2));
        }

        [Theory]
        [InlineData("Another")]
        [InlineData("NonSupported")]
        public void ShouldThrowExceptionForNonSupportedFont(string name)
        {
            Assert.Throws<FileNotFoundException>(() => MappingManager.GetFontToUnicodeMapping(name));
        }

        [Theory]
        [InlineData("AARituPlus2")]
        [InlineData("AARituPlus2-Numbers")]
        [InlineData("AARitu")]
        [InlineData("AAVishal")]
        [InlineData("Amita Times Cyr")]
        [InlineData("ThamesM")]
        [InlineData("ThamesSanskrit")]
        [InlineData("Amita Times")]
        [InlineData("Balaram")]
        [InlineData("DVRoman-TTSurekh")]
        [InlineData("GVPalatino")]
        [InlineData("Rama Garamond Plus")]
        [InlineData("ScaSeries")]
        [InlineData("SD1-TTSurekh")]
        public void ShouldReturnFontToUnicodeMapping(string fontName)
        {
            Assert.NotNull(MappingManager.GetFontToUnicodeMapping(fontName));
        }

        [Theory]
        [InlineData("Amita Times Cyr")]
        [InlineData("ThamesM")]
        [InlineData("ThamesSanskrit")]
        [InlineData("Amita Times")]
        [InlineData("Balaram")]
        [InlineData("DVRoman-TTSurekh")]
        [InlineData("GVPalatino")]
        [InlineData("Rama Garamond Plus")]
        [InlineData("ScaSeries")]
        [InlineData("SD1-TTSurekh")]
        public void ShouldReturnUnicodeToFontMapping(string fontName)
        {
            Assert.NotNull(MappingManager.GetUnicodeToFontMapping(fontName));
        }

        [Fact]
        public void ShouldReturnTranslitMappings()
        {
            Assert.NotNull(MappingManager.DevanagariToLatin);
            Assert.NotNull(MappingManager.LatinToCyrillic);
        }
    }
}
