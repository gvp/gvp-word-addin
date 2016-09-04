using GaudiaVedantaPublications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GaudiaVedantaPublications.Tests
{
    public class FontConversionTest
    {
        private static readonly IDictionary<string, IDictionary<string, string>> testData = new Dictionary<string, IDictionary<string, string>>
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
        };

        public static IEnumerable<object[]> ToUnicodeTestData
        {
            get
            {
                return
                    from fontName in testData.Keys
                    from entry in testData[fontName]
                    select new object[] { entry.Value, entry.Key, fontName };
            }
        }

        [Theory]
        [MemberData("ToUnicodeTestData")]
        public void ToUnicode(string unicode, string ascii, string fontName)
        {
            var map = MapManager.GetFontMap(fontName, MapDirection.Forward);
            Assert.Equal(unicode, map.Apply(ascii));
        }
    }
}