using System.Text;
using System.Text.Json;

namespace exemplo.api
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string endpoint);
        //Task<T> PostAsync<T>(string endpoint, object payload);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }

        //public async Task<T> PostAsync<T>(string endpoint, object payload)
        //{
        //    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PostAsync(endpoint, content);
        //    response.EnsureSuccessStatusCode();
        //    var json = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<T>(json);
        //}
    }
}
