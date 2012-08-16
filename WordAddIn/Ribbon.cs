using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Tools.Ribbon;

namespace VedicEditor
{
    public partial class Ribbon
    {
        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void Process(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.TransformText(GetTransforms().ToArray());
        }

        private IEnumerable<ITextTransform> GetTransforms()
        {
            yield return new ToUnicodeTransform();

            if (checkBoxDevanagari.Checked)
                yield return new DevanagariTransliterationTransform();

            if (checkBoxRoman.Checked)
                yield return new MapBasedTextTransform(MapManager.Lat2Cyr);

            if (dropDownFont.SelectedItemIndex > 0)
                yield return new FromUnicodeTransform(dropDownFont.SelectedItem.Label);
        }
    }
}
