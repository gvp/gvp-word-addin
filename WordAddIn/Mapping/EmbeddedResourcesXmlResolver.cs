using System;
using System.IO;
using System.Xml;

namespace GaudiaVedantaPublications
{
    internal class EmbeddedResourcesXmlResolver : XmlUrlResolver
    {
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri == null) throw new ArgumentNullException("absoluteUri", "Must provide an URI");

            return GetEntityFromEmbeddedResources(absoluteUri, ofObjectToReturn)
                ?? base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }

        private object GetEntityFromEmbeddedResources(Uri absoluteUri, Type ofObjectToReturn)
        {
            if (absoluteUri.Scheme != Uri.UriSchemeFile)
                return null;

            if (ofObjectToReturn != null && ofObjectToReturn != typeof(System.IO.Stream) && ofObjectToReturn != typeof(object))
                return null;

            return EmbeddedResourceManager.GetEmbeddedResource(Uri.UnescapeDataString(Path.GetFileName(absoluteUri.AbsolutePath)));
        }
    }
}
