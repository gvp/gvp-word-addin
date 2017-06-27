using GaudiaVedantaPublications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GaudiaVedantaPublications.Tests
{
    public class MapManagerTests
    {
        [Theory]
        [InlineData("ScaPalatino", "ScaGauda")]
        [InlineData("SDW", "SDW-Surekh")]
        [InlineData("KALAKAR", "KALAKAR1")]
        [InlineData("AARituPlus2", "AARituPlus2")]
        [InlineData("ThamesM", "ThamesM")]
        [InlineData("Amita Times Cyr", "Krishna Times Plus")]
        public void ShouldReturnSameMap(string name1, string name2)
        {
            Assert.Same(MapManager.GetFontToUnicodeMap(name1), MapManager.GetFontToUnicodeMap(name2));
        }

        [Theory]
        [InlineData("Something", "Another")]
        [InlineData("NonExistent", "Unicode")]
        public void ShouldReturnNullForNonExistent(string source, string destination)
        {
            Assert.Equal(null, MapManager.GetMap(source, destination));
        }

        [Theory]
        [InlineData("AARituPlus2", "Unicode")]
        [InlineData("AARituPlus2-Numbers", "Unicode")]
        [InlineData("AARitu", "Unicode")]
        [InlineData("AAVishal", "Unicode")]
        [InlineData("Amita Times Cyr", "Unicode")]
        [InlineData("ThamesM", "Unicode")]
        [InlineData("ThamesSanskrit", "Unicode")]
        [InlineData("Amita Times", "Unicode")]
        [InlineData("Balaram", "Unicode")]
        [InlineData("DVRoman-TTSurekh", "Unicode")]
        [InlineData("GVPalatino", "Unicode")]
        [InlineData("Rama Garamond Plus", "Unicode")]
        [InlineData("ScaSeries", "Unicode")]
        [InlineData("SD1-TTSurekh", "Unicode")]
        [InlineData("Unicode", "Amita Times Cyr")]
        [InlineData("Unicode", "ThamesM")]
        [InlineData("Unicode", "ThamesSanskrit")]
        [InlineData("Unicode", "Amita Times")]
        [InlineData("Unicode", "Balaram")]
        [InlineData("Unicode", "DVRoman-TTSurekh")]
        [InlineData("Unicode", "GVPalatino")]
        [InlineData("Unicode", "Rama Garamond Plus")]
        [InlineData("Unicode", "ScaSeries")]
        [InlineData("Unicode", "SD1-TTSurekh")]
        [InlineData("Devanagari", "Latin")]
        [InlineData("Latin", "Cyrillic")]
        public void ShouldReturnMap(string source, string destination, int count)
        {
            Map map = MapManager.GetMap(source, destination);
            Assert.NotNull(map);
        }
    }
}