using Microsoft.Office.Tools.Ribbon;

namespace VedicEditor
{
    public partial class Ribbon
    {
        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void buttonConvertToThamesM_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.TransformText(
                new ToUnicodeTransform(),
                new Lat2CyrTransform(),
                new FromUnicodeTransform("ThamesM")
                );
        }

        private void buttonConvertToUnicode_Click(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.TransformText(new ToUnicodeTransform());
        }
    }
}
