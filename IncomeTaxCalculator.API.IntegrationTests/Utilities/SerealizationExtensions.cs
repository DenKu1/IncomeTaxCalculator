using Newtonsoft.Json;
using System.Text;

namespace IncomeTaxCalculator.API.IntegrationTests.Utilities
{
    public static class SerealizationExtensions
    {
        public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage message)
        {
            var stringResponse = await message.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResponse);
        }

        public static HttpContent Serealize<T>(this T entity) where T : class
        {
            var jsonString = JsonConvert.SerializeObject(entity);
            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }
    }
}
