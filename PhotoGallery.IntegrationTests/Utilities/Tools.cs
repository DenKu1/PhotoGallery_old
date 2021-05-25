using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PhotoGallery.IntegrationTests.Utilities
{
    public static class Tools
    {
        public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage message)
        {
            var stringResponse = await message.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResponse);
        }

    }
}
