using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

namespace VedicEditor
{
    public partial class Ribbon
    {
        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
            foreach (var fontName in FontTransformer.CyrillicFonts)
            {
                var item = Factory.CreateRibbonDropDownItem();
                item.Label = fontName;
                dropDownFont.Items.Add(item);
            }
            dropDownFont.SelectedItem = dropDownFont.Items.Where(x => x.Label == "ThamesM").FirstOrDefault();
        }

        private void buttonLaunch_Click(object sender, RibbonControlEventArgs e)
        {
            var transformer = new FontTransformer(dropDownFont.SelectedItem.Label);
            transformer.Transform();
        }
    }
}
