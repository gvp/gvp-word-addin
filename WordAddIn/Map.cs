using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GaudiaVedantaPublications
{
    public abstract class Map
    {
        public static Map Create(XElement element)
        {
            var from = element.AttributeValue("from").Normalize(System.Text.NormalizationForm.FormC);
            var to = element.AttributeValue("to").Normalize(System.Text.NormalizationForm.FormC);
            var order = Int32.Parse(element.AttributeValue("order", "0"));

            switch (element.AttributeValue("type"))
            {
                case "regex":
                    return new RegexMap(from, to);

                default:
                    return new ReplaceMap(from, to);
            }
        }

        public abstract string Apply(string text);

        public abstract Map Inverted
        {
            get;
        }
    }
}
