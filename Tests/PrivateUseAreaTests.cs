using System.Linq;
using NUnit.Framework;

namespace GaudiaVedantaPublications.Tests
{
    [Category("PUA")]
    public class PrivateUseAreaTests
    {
        [Test]
        public void PrivateUseAreaShouldMapToAnsi()
        {
            foreach (int index in Enumerable.Range(0x20, 0xFF - 0x20))
            {
                var ansi = (char)(index);
                var puaE = ((char)(index + 0xE000));
                var puaF = ((char)(index + 0xF000));
                Assert.AreEqual(ansi, puaE.PrivateUseAreaToAnsi());
                Assert.AreEqual(ansi, puaF.PrivateUseAreaToAnsi());
            }
        }

        [TestCase("\x006B")]
        [TestCase("\x0403")]
        public void StringWithoutPUA(string source)
        {
            Assert.AreSame(source, source.PrivateUseAreaToAnsi());
        }

        [TestCase("\xF06B\x0403\xE045\x006B", "\x006B\x0403\x0045\x006B")]
        public void StringWithPUA(string source, string expected)
        {
            Assert.AreEqual(expected, source.PrivateUseAreaToAnsi());
        }
    }
}
