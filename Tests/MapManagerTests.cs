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
        [InlineData("AARituPlus2", "Unicode", 197 + 30)]
        [InlineData("Amita Times Cyr", "Unicode", 43)]
        [InlineData("ThamesM", "Unicode", 46)]
        [InlineData("ThamesSanskrit", "Unicode", 126)]
        [InlineData("DVRoman-TTSurekh", "Unicode", 59)]
        [InlineData("Unicode", "Amita Times Cyr", 43)]
        [InlineData("Unicode", "ThamesM", 46)]
        [InlineData("Unicode", "ThamesSanskrit", 126)]
        [InlineData("Unicode", "DVRoman-TTSurekh", 59)]
        [InlineData("Devanagari", "Latin", 92)]
        [InlineData("Latin", "Cyrillic", 106)]
        public void ShouldReturnMap(string source, string destination, int count)
        {
            var map = MapManager.GetMap(source, destination);
            Assert.NotNull(map);
            Assert.IsAssignableFrom<IOrderedEnumerable<MapEntry>>(map);
            Assert.Equal(count, map.Count());
        }
    }
}