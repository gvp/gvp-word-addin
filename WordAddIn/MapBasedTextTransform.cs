using System.Linq;
using System.Text;
using Map = System.Collections.Generic.IEnumerable<GaudiaVedantaPublications.MapEntry>;
using Word = Microsoft.Office.Interop.Word;

namespace GaudiaVedantaPublications
{
    class MapBasedTextTransform : ITextTransform
    {
        public MapBasedTextTransform()
        {
        }

        public MapBasedTextTransform(Map map)
        {
            CurrentMap = map;
        }

        protected Map CurrentMap { get; set; }

        public virtual void Apply(Word.Range range)
        {
            var text = range.Text.PUAToASCII();

            foreach (var entry in CurrentMap.OrderBy(e => e.Order))
                text = entry.Apply(text);

            range.Text = text;
        }
    }
}
