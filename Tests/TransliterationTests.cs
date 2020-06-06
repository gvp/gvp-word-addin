using System.Text;
using Xunit;

namespace GaudiaVedantaPublications.Tests
{
    [Trait("Category", "Tranliteration")]
    public class TransliterationTests
    {
        [Theory]
        [InlineData("वैष्णव–सिद्धान्त–माला", "vaiṣṇava–siddhānta–mālā")]
        [InlineData("ॐ", "oṁ")]
        [InlineData("क्ष", "kṣa")]
        [InlineData("त्र", "tra")]
        [InlineData("ज्ञ", "jña")]
        [InlineData("श्र", "śra")]
        [InlineData("ड़", "ṛa")]
        [InlineData("श्रीगौड़ीयाचार्य", "śrīgauṛīyācārya")]
        [InlineData("पाँच", "pām̐ca")]
        public void DevanagariToLatin(string devanagari, string latin)
        {
            Assert.Equal(latin.Normalize(NormalizationForm.FormC), MappingManager.DevanagariToLatin.Apply(devanagari));
        }

        [Theory]
        [InlineData("vaiṣṇava–siddhānta–mālā", "ваиш̣н̣ава–сиддха̄нта–ма̄ла̄")]
        [InlineData("oṁ", "ом̇")]
        [InlineData("evam", "эвам")]
        [InlineData("sevonmukhe", "севонмукхе")]
        [InlineData("bhaktiprajñāna", "бхактипраджн̃а̄на")]
        public void LatinToCyrillic(string latin, string cyrillic)
        {
            Assert.Equal(cyrillic.Normalize(NormalizationForm.FormC), MappingManager.LatinToCyrillic.Apply(latin));
        }
    }
}
