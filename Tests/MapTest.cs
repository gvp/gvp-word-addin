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
            { "AARituPlus2", new Dictionary<string, string> {
                { ",", "ए" },
                { "[", "ख्" },
                { "ý", "[" },
                { "»", "÷" },
                { "õ", "[GVP logo white]" },
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
                { "tM+cfq¼ls", "जड़बुद्धिसे" }, /// Wrong order of ु and ि
                { "D;kas", "क्यों" }, /// Wrong order of ो and ं
                { "gaS", "हैं" }, /// Wrong order of ै and ं
                { "nq;kZsèkuk|k%", "दुर्योधनाद्याः" }, /// Wrong order of र् and े
                { "dÙk±q", "कर्त्तुं" }, /// Wrong order of ु and ं
                { "'kÌknhuka", "शङ्खादीनां" },
                { "Hksndks", "भेदको" }, /// 
                { "rstks·a'klaHkoe~", "तेजोंऽशसंभवम्" }, /// BG page 107 ॥४१॥ Wrong order of anusvar and apostrophe http://gitabase.com/the/gita/eng/BG/10/41
                { "'qkp%", "शुचः" }, /// BG page 146 ॥५॥. Wrong order of u-sign and a-sign.
                { "oiqnZèkr*~*", "वपुर्दधत्’’" }, /// BG page 169 ॥५५॥. Wrong order of virama and apostrophe.
                { "izofÙZkrqa", "प्रवर्त्तितुं" }, /// BG page 170 ॥५९॥. Wrong order of Z and something
                { "xhkr'kkL=L;kfi", "गीतशास्त्रस्यापि" }, /// BG page 172 ॥६४॥. Mistaken a-sign after i-sign.
                { "ughsa", "नहीं" }, /// Extra े
                { "vkSj", "और" }, /// 
                { "Ük`Âkjjfr", "शृङ्गाररति" }, /// 
                { "¥glk", "हिंसा" }, /// 
                { "nksuksa", "दोनों" }, /// 
                { "lkefxz;ksasds", "सामग्रियोंके" }, /// Extra े
                { "tM+cqf¼okyss", "जड़बुद्धिवाले" }, /// Extra े
                { "oS\".koèkeZesaa", "वैष्णवधर्ममें" }, /// Extra ं
                { "voLFkkessa", "अवस्थामें" }, /// Extra े
                { "O;fä;ksaesaaa", "व्यक्तियोंमें" }, /// Three ं
                { "iq#\"k'pkkfèknSore~", "पुरुषश्चाधिदैवतम्" }, /// BG page 80 ॥४॥ double a-sign
                { "dk;Zdkj.kdÙkZZ`ZRos", "कार्यकारणकर्त्तृत्वे" }, /// BG page 129 ॥२१॥ Double Z. Wrong order of `Z.
                { "rn~~?kua", "तद्‌घनं" }, /// BG page 138 ॥२७॥ Double virama.
                { "ozrkkfu", "व्रतानि" }, /// BG page 148 ॥१०॥. Triple a-sign.
                { ";rLrr~~", "यतस्तत्" }, /// BG page 156 ॥२८॥ Double virama
                { "izzzzzzzzzzzzzzzzzzzÏreuqljke%", "प्रकृतमनुसरामः" }, /// BG page 161 ॥१८॥ Many zzzzz
                { "dk;ZsZ", "कार्ये" }, /// BG page 162 ॥२२॥. Extra Z.
                { "fuxZZzUFkk", "निर्ग्रन्था" }, /// BG page 169 ॥५५॥. Extra Z
                { "Hkxoku~dk", "भगवान्\u200Cका" }, /// Explicit halant, see http://unicode.org/faq/indic.html#2
                { "Hkxoku~ gh", "भगवान् ही" }, /// Explicit halant in the end of the word
                { "QykdkfÀ.kks", "फलाकाङ्क्षिणो" }, /// BG page 23 ॥४७॥
                { "lEeksgkr~~","सम्मोहात्" }, // BG page 26 ॥६३॥
                { "Js;ku~+", "श्रेयाऩ्" }, /// BG page 37 ॥३५॥
                { "prqFkkZss", "चतुर्थो" }, /// BG page 51 ॥४२॥
                { "fpÙk'kqf¼nk<~îkZTKkunk<~îeso", "चित्तशुद्धिदार्ढ्याज्ज्ञानदाढ्यमेव" }, /// BG page 52 ॥२॥
                { "cq¼îkRekdkj;Sso", "बुद्ध्यात्माकारयैव" }, /// BG page 62 ॥२०–२५॥
                { "ukjk;.kewfÙZkjso", "नारायणमूर्त्तिरेव" }, /// BG page 74 ॥१५॥
                { "deZZRo", "कर्मत्व" }, /// BG page 74 ॥१६॥
                { "en~;ksxkdkfÀ.k%", "मद्‌योगाकाङ्क्षिणः" }, /// BG page 82 ॥१४॥
                { "eRla;ksxkdkfÀ.kka", "मत्संयोगाकाङ्क्षिणां" }, /// BG page 101 ॥१०॥
                { "xHkZ±", "गर्भं" }, /// BG page 133 ॥३॥
                { "la{skis.k", "संक्षेपेण" }, /// BG page 143 ॥१६॥
                { "oÙZkrs", "वर्त्तते" }, /// BG page 155 ॥२६॥
            }},
            { "AARitu", new Dictionary<string, string> {
                { ",", "ए" },
                { "[", "ख्" },
                { "ý", "[" },
                { "»", "÷" },
                { "¹", "★" },
                { "õ", "࿗" },
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
            }},
            { "Amita Times Cyr", new Dictionary<string, string> {
                { "к¦п…м¦т…бдхе±", "кр̣па̄мр̣та̄бдхеш́" },
                { "ѕ", "[lotus]" },
                { "\x00AD", "ӯ" },
                { "\x00B7", "Т̣" },
                { "\x0088", "л̣" },
                { "\x0098", "м̇" },
                { "\x0457", "\x0301" },
            }},
            { "ThamesM", new Dictionary<string, string> {
                {"iрb-iука{ увfча", "ш́рӣ-ш́уках̣ ува̄ча" },
                {"iрb iрb гуру-гаурfyга джаята{", "ш́рӣ ш́рӣ гуру-гаура̄н̇га джаятах̣" },
                {"виджufйа", "виджн̃а̄йа" },
                {"сарва-бхeта-хhдайаv", "сарва-бхӯта-хр̣дайам̇" },
                { "g", "р̣̄" },
                { "u", "н̃" },
                { "z", "у̃" },
            }},
            { "ThamesSanskrit", new Dictionary<string, string> {
                { "äæufíàv òå ’õàv ñà-âèäæufíàì, èäàv âàêøéfìé àiåøàòà{", "джн̃а̄нам̇ те ’хам̇ са-виджн̃а̄нам, идам̇ вакшйа̄мй аш́ешатах̣" },
                { "éàäæ äæufòâf íåõà áõeéî ’íéàäæ, äæufòàâéàì àâàièøéàòå", "йадж джн̃а̄тва̄ неха бхӯйо ’нйадж, джн̃а̄тавйам аваш́ишйате" },
                { "uf", "н̃а̄" },
                { "Ÿ", "[и-с-тильдой-снизу]" },
            }},
            { "Amita Times", new Dictionary<string, string> {
                { "çréçréguru-gauräìgau jayataù", "śrīśrīguru-gaurāṅgau jayataḥ" },
                { "Ï", "Ñ" },
                { "Î", "Ã" },
                { "¾", "[tilak]" },
                { "·", "[lotus]" },
                { "Í", "ī̃" },
                { "é", "ī" },
            }},
            { "Balaram", new Dictionary<string, string> {
                { "mäträ-sparçäs tu kaunteya çétoñëa-sukha-duùkha-däù", "mātrā-sparśās tu kaunteya śītoṣṇa-sukha-duḥkha-dāḥ" },
                { "Ï", "Ñ" },
                { "ß", "Ḷ" },
            }},
            { "DVRoman-TTSurekh", new Dictionary<string, string> {
                { "¿rî¿rîguru-gaur¡´gau jayataÅ", "śrīśrīguru-gaurāṅgau jayataḥ" },
                { "Ø", "Ñ" },
            }},
            { "Rama Garamond Plus", new Dictionary<string, string> {
                { "çréçréguru-gauräìgau jayataù", "śrīśrīguru-gaurāṅgau jayataḥ" },
                { "Ï", "Ñ" },
                { "Î", "Ã" },
                { "¾", "[tilak]" },
                { "·", "[lotus]" },
                { "Í", "ī̃" },
                { "é", "ī" },
            }},
            { "Sca", new Dictionary<string, string> {
                { "çréçréguru-gauräìgau jayataù", "śrīśrīguru-gaurāṅgau jayataḥ" },
                { "ï", "ñ" },
                { "ñ", "ṣ" },
                { "Ω", "ë" },
                { "Δ", "Ü" },
                { "≈", "È" },
            }},
            { "SD1-TTSurekh", new Dictionary<string, string> {
                { "¿r¢¿r¢guru-gaur¡´gau jayataÅ", "śrīśrīguru-gaurāṅgau jayataḥ" },
                { "Ø", "Ñ" },
            }},
            { "GVPalatino", new Dictionary<string, string> {
                { "çréçréguru-gauräìgau jayataù", "śrīśrīguru-gaurāṅgau jayataḥ" },
                { "ï", "ñ" },
                { "ñ", "ṣ" },
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
                return GetFontTestData(excludeFontNames: MapManager.DevanagariFonts);
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