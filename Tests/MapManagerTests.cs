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
        [InlineData("ThamesM", "Unicode")]
        public void ShouldReturnOrderedEnumerable(string source, string destination)
        {
            Assert.IsAssignableFrom<IOrderedEnumerable<MapEntry>>(MapManager.GetMap(source, destination));
        }

        [Theory]
        [InlineData("ThamesM", "Unicode", 46)]
        public void MapShouldContainExactNumberOfEntries(string source, string destination, int count)
        {
            Assert.Equal(count, MapManager.GetMap(source, destination).Count());
        }

    }
}