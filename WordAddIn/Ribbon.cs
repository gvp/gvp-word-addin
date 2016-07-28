using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using stdole;
using Office = Microsoft.Office.Core;

namespace GaudiaVedantaPublications
{
    [ComVisible(true)]
    public partial class Ribbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public Ribbon()
        {
        }

        public string GetCustomUI(string ribbonID)
        {
            return EmbeddedResourceManager.GetEmbeddedStringResource("Ribbon.xml");
        }

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, select the Ribbon XML item in Solution Explorer and then press F1

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public IPictureDisp LoadImage(string imageId)
        {
            var image = EmbeddedResourceManager.GetEmbeddedImageResource(String.Format("Images.{0}", imageId));
            if (image == null)
                return null;
            return PictureDispConverter.ToIPictureDisp(image);
        }

        public string GetLabel(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Label");
        }

        public string GetTitle(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Title");
        }

        public string GetDescription(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Description");
        }

        public string GetScreentip(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Screentip");
        }

        public string GetSupertip(IRibbonControl control)
        {
            return GetResourceString(control.Id, "Supertip");
        }

        private static string GetResourceString(string id, string attribute)
        {
            return Properties.Resources.ResourceManager.GetString(String.Format("{0}_{1}", id, attribute));
        }

        #endregion
    }
}
