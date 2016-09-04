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
            Assert.Same(MapManager.GetFontMap(name1, MapDirection.Forward), MapManager.GetFontMap(name2, MapDirection.Forward));
        }

        [Theory]
        [InlineData("Something")]
        [InlineData("NonExistent")]
        public void ShouldReturnNullForNonExistent(string name)
        {
            Assert.Equal(null, MapManager.GetFontMap(name, MapDirection.Forward));
        }

        [Theory]
        [InlineData("AARituPlus2")]
        [InlineData("ThamesM")]
        public void ShouldReturnOrderedEnumerable(string name)
        {
            Assert.IsAssignableFrom<IOrderedEnumerable<MapEntry>>(MapManager.GetFontMap(name, MapDirection.Forward));
        }
    }
}