using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GaudiaVedantaPublications.Tests
{
    internal class TestDataLoader
    {
        public static JObject ReadTestData(string name)
        {
            using (var resource = EmbeddedResourceManager.GetEmbeddedResource(name))
            using (var reader = new StreamReader(resource))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return JObject.Load(jsonReader);
            }
        }
    }
}
