using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace GaudiaVedantaPublications
{
    internal static class EmbeddedResourceManager
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

        public static Stream GetEmbeddedResource(String resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(typeof(EmbeddedResourceManager), resourceName);
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
