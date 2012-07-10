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
        }

        private void buttonConvertToThamesM_Click(object sender, RibbonControlEventArgs e)
        {
            var transformer = new FontTransformer("ThamesM");
            transformer.Transform();
        }
    }
}
