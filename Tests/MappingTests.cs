﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GaudiaVedantaPublications.Tests
{
    [Trait("Category", "Mapping")]
    public class MappingTests
    {
        private static IEnumerable<object[]> GetFontTestData(params string[] includeFontNames)
        {
            return
                from set in TestDataLoader.ReadTestData("FontMappingTestData.json")["sets"]
                /// Either taking fonts array of the set, or resort to the set name.
                from fontName in set["fonts"] ?? Enumerable.Repeat(set["name"], 1)
                where includeFontNames == null || includeFontNames.Contains((string)fontName)
                from entry in set["entries"]
                select new object[] { (string)entry["to"], (string)entry["from"], (string)fontName };
        }

        public static IEnumerable<object[]> FontToUnicodeTestData
        {
            get
            {
                return GetFontTestData("AARitu");
            }
        }

        [Theory]
        [Trait("Mapping", "ToUnicode")]
        [MemberData(nameof(FontToUnicodeTestData))]
        public void ToUnicode(string unicode, string ansi, string fontName)
        {
            var map = MappingManager.GetFontToUnicodeMapping(fontName);
            Assert.Equal(unicode.Normalize(NormalizationForm.FormC), map.Apply(ansi));
        }

        public static IEnumerable<object[]> UnicodeToFontTestData
        {
            get
            {
                //return GetFontTestData(excludeFontNames: MappingManager.DevanagariFonts);
                return GetFontTestData();
            }
        }

        [Theory]
        [Trait("Mapping", "FromUnicode")]
        [MemberData(nameof(UnicodeToFontTestData))]
        public void FromUnicode(string unicode, string ansi, string fontName)
        {
            var map = MappingManager.GetUnicodeToFontMapping(fontName);
            Assert.Equal(ansi.Normalize(NormalizationForm.FormC), map.Apply(unicode));
        }

        [Theory]
        [Trait("Mapping", "Transliteration")]
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
        [Trait("Mapping", "Transliteration")]
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