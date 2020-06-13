using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GaudiaVedantaPublications
{
    public static class EmbeddedResourceManager
    {
        internal class PictureConverter : System.Windows.Forms.AxHost
        {
            private PictureConverter() : base(String.Empty) { }

            static public stdole.IPictureDisp ImageToPictureDisp(System.Drawing.Image image)
            {
                return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
            }

            static public stdole.IPictureDisp IconToPictureDisp(System.Drawing.Icon icon)
            {
                return ImageToPictureDisp(icon.ToBitmap());
            }

            static public System.Drawing.Image PictureDispToImage(stdole.IPictureDisp picture)
            {
                return GetPictureFromIPicture(picture);
            }
        }

        public static Stream GetEmbeddedResource(string name, Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetCallingAssembly();
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(name));
            if (resourceName == null)
                return null;
            return assembly.GetManifestResourceStream(resourceName);
        }

        public static String GetEmbeddedStringResource(String resourceName)
        {
            using (var stream = GetEmbeddedResource(resourceName))
            {
                if (stream == null)
                    return null;
                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }

        public static Image GetEmbeddedImageResource(String resourceName)
        {
            using (var stream = GetEmbeddedResource(resourceName))
            {
                if (stream == null)
                    return null;
                return Image.FromStream(stream);
            }
        }
    }
}
