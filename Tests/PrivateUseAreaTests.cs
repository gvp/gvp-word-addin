using System.Linq;
using Xunit;

namespace GaudiaVedantaPublications.Tests
{
    public class PrivateUseAreaTests
    {
        [Fact]
        public void PrivateUseAreaShouldMapToAnsi()
        {
            foreach (int index in Enumerable.Range(0x20, 0xFF - 0x20))
            {
                var ansi = (char)(index);
                var puaE = ((char)(index + 0xE000));
                var puaF = ((char)(index + 0xF000));
                Assert.Equal(ansi, puaE.PrivateUseAreaToAnsi());
                Assert.Equal(ansi, puaF.PrivateUseAreaToAnsi());
            }
        }

        [Theory]
        [InlineData("\x006B")]
        [InlineData("\x0403")]
        public void StringWithoutPUA(string source)
        {
            Assert.Same(source, source.PrivateUseAreaToAnsi());
        }

        [Theory]
        [InlineData("\xF06B\x0403\xE045\x006B", "\x006B\x0403\x0045\x006B")]
        public void StringWithPUA(string source, string expected)
        {
            Assert.Equal(expected, source.PrivateUseAreaToAnsi());
        }
    }
}
