using GaudiaVedantaPublications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GaudiaVedantaPublications.Tests
{
    public class MapTest
    {
        private static readonly IDictionary<string, IDictionary<string, string>> fontTestData = new Dictionary<string, IDictionary<string, string>>
        {
            { "AARituPlus2", new Dictionary<string, string>
                {
                    { "JhJhxq#&xkSjkÂkS t;r%", "श्रीश्रीगुरु–गौराङ्गौ जयतः" },
                    { "bZn`'kks", "ईदृशो" },
                    { "izsenkukorh.kksZ", "प्रेमदानावतीर्णो" },
                    { "bR;FkZ%", "इत्यर्थः" },
                    { "ijenqyZHkRokr~", "परमदुर्लभत्वात्" },
                    { "uhZpSjfi", "र्नीचैरपि" },
                    { "loSZxksZihfo\"k;da", "सर्वैर्गोपीविषयकं" },
                    { "iq#\"kkFkks±dh", "पुरुषार्थोंकी" },
                    { "^", "‘" },
                    { "fu£o'ks\"kcqf¼", "निर्विशेषबुद्धि" },
                    {"tM+cfq¼ls", "जड़बुद्धिसे" }, /// Wrong order of ु and ि
                    {"D;kas", "क्यों" }, /// Wrong order of ो and ं
                    {"gaS", "हैं" }, /// Wrong order of ै and ं
                    {"Hksndks", "भेदको" }, /// 
                    {"ughsa", "नहीं" }, /// Extra े
                    {"vkSj", "और" }, /// 
                    {"Ük`Âkjjfr", "शृङ्गाररति" }, /// 
                    {"¥glk", "हिंसा" }, /// 
                    {"nksuksa", "दोनों" }, /// 
                    {"lkefxz;ksasds", "सामग्रियोंके" }, /// Extra े
                    {"tM+cqf¼okyss", "जड़बुद्धिवाले" }, /// Extra े
                    {"oS\".koèkeZesaa", "वैष्णवधर्ममें" }, /// Extra ं
                    {"voLFkkessa", "अवस्थामें" }, /// Extra े
                    {"O;fä;ksaesaaa", "व्यक्तियोंमें" }, /// Three ं
                }
            },
            { "Amita Times Cyr", new Dictionary<string, string> {
                { "к¦п…м¦т…бдхе±", "кр̣па̄мр̣та̄бдхеш́" },
            }},
            { "ThamesM", new Dictionary<string, string>
                {
                    {"iрb-iука{ увfча", "ш́рӣ-ш́уках̣ ува̄ча" },
                    {"iрb iрb гуру-гаурfyга джаята{", "ш́рӣ ш́рӣ гуру-гаура̄н̇га джаятах̣" },
                    {"виджufйа", "виджн̃а̄йа" },
                    {"сарва-бхeта-хhдайаv", "сарва-бхӯта-хр̣дайам̇" },
                }
            },
            { "ThamesSanskrit", new Dictionary<string, string> {
                { "äæufíàv òå ’õàv ñà-âèäæufíàì, èäàv âàêøéfìé àiåøàòà{", "джн̃а̄нам̇ те ’хам̇ са-виджн̃а̄нам, идам̇ вакшйа̄мй аш́ешатах̣" },
                { "éàäæ äæufòâf íåõà áõeéî ’íéàäæ, äæufòàâéàì àâàièøéàòå", "йадж джн̃а̄тва̄ неха бхӯйо ’нйадж, джн̃а̄тавйам аваш́ишйате" },
                { "uf", "н̃а̄" },
            }},
            { "DVRoman-TTSurekh", new Dictionary<string, string> {
                { "¿rî¿rîguru-gaur¡´gau jayataÅ", "śrīśrīguru-gaurāṅgau jayataḥ" }
            }},
            { "SD1-TTSurekh", new Dictionary<string, string> {
                { "¿r¢¿r¢guru-gaur¡´gau jayataÅ", "śrīśrīguru-gaurāṅgau jayataḥ" }
            }},
        };

        private static IEnumerable<object[]> GetFontTestData(params string[] excludeFontNames)
        {
            return
                from fontName in fontTestData.Keys.Except(excludeFontNames)
                from entry in fontTestData[fontName]
                select new object[] { entry.Value, entry.Key, fontName };
        }

        public static IEnumerable<object[]> FontToUnicodeTestData
        {
            get
            {
                return GetFontTestData();
            }
        }

        [Theory]
        [MemberData("FontToUnicodeTestData")]
        public void ToUnicode(string unicode, string from, string fontName)
        {
            var map = MapManager.GetFontToUnicodeMap(fontName);
            Assert.Equal(unicode.Normalize(NormalizationForm.FormC), map.Apply(from));
        }

        public static IEnumerable<object[]> UnicodeToFontTestData
        {
            get
            {
                return GetFontTestData(excludeFontNames: "AARituPlus2");
            }
        }

        [Theory]
        [MemberData("UnicodeToFontTestData")]
        public void FromUnicode(string unicode, string to, string fontName)
        {
            var map = MapManager.GetUnicodeToFontMap(fontName);
            Assert.Equal(to.Normalize(NormalizationForm.FormC), map.Apply(unicode));
        }

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
            Assert.Equal(latin.Normalize(NormalizationForm.FormC), MapManager.DevanagariToLatin.Apply(devanagari));
        }

        [Theory]
        [InlineData("vaiṣṇava–siddhānta–mālā", "ваиш̣н̣ава–сиддха̄нта–ма̄ла̄")]
        [InlineData("oṁ", "ом̇")]
        [InlineData("evam", "эвам")]
        [InlineData("sevonmukhe", "севонмукхе")]
        [InlineData("bhaktiprajñāna", "бхактипраджн̃а̄на")]
        public void LatinToCyrillic(string latin, string cyrillic)
        {
            Assert.Equal(cyrillic.Normalize(NormalizationForm.FormC), MapManager.LatinToCyrillic.Apply(latin));
        }
    }
}