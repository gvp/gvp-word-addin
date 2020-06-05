using System.Text;
using NUnit.Framework;

namespace GaudiaVedantaPublications.Tests
{
    [Category("Tranliteration")]
    public class TransliterationTests
    {
        [TestCase("वैष्णव–सिद्धान्त–माला", "vaiṣṇava–siddhānta–mālā")]
        [TestCase("ॐ", "oṁ")]
        [TestCase("क्ष", "kṣa")]
        [TestCase("त्र", "tra")]
        [TestCase("ज्ञ", "jña")]
        [TestCase("श्र", "śra")]
        [TestCase("ड़", "ṛa")]
        [TestCase("श्रीगौड़ीयाचार्य", "śrīgauṛīyācārya")]
        [TestCase("पाँच", "pām̐ca")]
        public void DevanagariToLatin(string devanagari, string latin)
        {
            Assert.AreEqual(latin.Normalize(NormalizationForm.FormC), MappingManager.DevanagariToLatin.Apply(devanagari));
        }

        [TestCase("vaiṣṇava–siddhānta–mālā", "ваиш̣н̣ава–сиддха̄нта–ма̄ла̄")]
        [TestCase("oṁ", "ом̇")]
        [TestCase("evam", "эвам")]
        [TestCase("sevonmukhe", "севонмукхе")]
        [TestCase("bhaktiprajñāna", "бхактипраджн̃а̄на")]
        public void LatinToCyrillic(string latin, string cyrillic)
        {
            Assert.AreEqual(cyrillic.Normalize(NormalizationForm.FormC), MappingManager.LatinToCyrillic.Apply(latin));
        }
    }
}
