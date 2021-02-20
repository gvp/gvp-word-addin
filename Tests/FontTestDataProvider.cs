using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Mvp.Xml.XInclude;
using NUnit.Framework;

namespace GaudiaVedantaPublications.Tests
{
    public enum FontConversionDirection
    {
        LocalToUnicode,
        UnicodeToLocal
    }

    public static class FontTestDataProvider
    {
        private static IEnumerable<string> GetFontNames(FontConversionDirection direction)
        {
            yield return "Amita Times Cyr";
            yield return "ThamesM";
            yield return "ThamesSanskrit";
            yield return "Amita Times";
            yield return "Balaram";
            yield return "DVRoman-TTSurekh";
            yield return "GVPalatino";
            yield return "Rama Garamond Plus";
            yield return "ScaSeries";
            yield return "SD1-TTSurekh";
            yield return "AARituPlus2";

            /// Some fonts allow only one direction of conversion: to unicode
            if (direction != FontConversionDirection.LocalToUnicode) yield break;

            yield return "AARituPlus2-Numbers";
            yield return "AARitu";
            yield return "AAVishal";
        }

        public static IEnumerable<object[]> GetFontNamesForConversion(FontConversionDirection direction) =>
            GetFontNames(direction).Select(fontName => new object[] { fontName });

        private static readonly XmlResolver xmlResolver = new EmbeddedResourcesXmlResolver();

        private static XElement LoadTestSet(string fontName)
        {
            using (var reader = new XIncludingReader(fontName + ".xml", xmlResolver))
            {
                /// Setting the resolver here according to http://mvp-xml.sourceforge.net/xinclude/#mozTocId795134
                /// Passing as a parameter is also required to resolve the original file
                reader.XmlResolver = xmlResolver;
                return XElement.Load(reader);
            }
        }

        public static IEnumerable<TestCaseData> GetFontTestData(FontConversionDirection direction) =>
            from fontName in GetFontNames(direction)
            from set in LoadTestSet(fontName).DescendantsAndSelf("set")
            from @case in set.Elements("case")
            let caseDirection = @case.Attribute("direction")?.Value ?? "both"
            where caseDirection == "both" || caseDirection == (direction == FontConversionDirection.LocalToUnicode ? "to-unicode" : "from-unicode")
            orderby fontName
            let unicode = @case.Attribute("unicode")?.Value.Normalize(NormalizationForm.FormC)
            let local = @case.Attribute("local")?.Value.Normalize(NormalizationForm.FormC)
            let input = direction == FontConversionDirection.LocalToUnicode ? local : unicode
            let output = direction == FontConversionDirection.LocalToUnicode ? unicode : local
            select new TestCaseData(fontName, input)
                .Returns(output)
                .SetProperty("Font Name", fontName)
                .SetProperty("Set",
                    set.Attribute("name")?.Value ??
                    set.Attribute(XNamespace.Xml + "base")?.Value?.Replace(".xml", string.Empty) ??
                    fontName)
                .SetArgDisplayNames(fontName, @case.Attribute("comment")?.Value ?? unicode)
                ;

    }
}
