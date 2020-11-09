using System.Linq;
using NUnit.Framework;

namespace GaudiaVedantaPublications.Tests
{
    [Category("PUA")]
    public class PrivateUseAreaTests
    {
        [Test]
        public void PrivateUseAreaE000AndF000ShouldMapTo8Bit()
        {
            foreach (int index in Enumerable.Range(0x20, 0xFF - 0x20))
            {
                var characterIn8Bit = (char)(index);
                var characterInE000 = ((char)(index + 0xE000));
                var characterInF000 = ((char)(index + 0xF000));
                Assert.AreEqual(characterIn8Bit, characterInE000.PrivateUseAreaTo8Bit());
                Assert.AreEqual(characterIn8Bit, characterInF000.PrivateUseAreaTo8Bit());
            }
        }

        [TestCase("\x006B")]
        [TestCase("\x0403")]
        public void StringWithoutPUAShouldNotChange(string source)
        {
            Assert.AreSame(source, source.PrivateUseAreaTo8Bit());
        }

        [TestCase("\xF06B\x0403\xE045\x006B", "\x006B\x0403\x0045\x006B")]
        public void PUAMixedWithOthersShouldMap(string source, string expected)
        {
            Assert.AreEqual(expected, source.PrivateUseAreaTo8Bit());
        }
    }
}
