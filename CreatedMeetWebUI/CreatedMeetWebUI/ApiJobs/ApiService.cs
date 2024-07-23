using Newtonsoft.Json;

namespace CreatedMeetWebUI.ApiJobs
{
    public class ApiService : IApiService
    {
        private static readonly Lazy<ApiService> instance = new Lazy<ApiService>(() => new ApiService(new HttpClient()));
        private readonly HttpClient _httpClient;


        private ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public static ApiService Instance => instance.Value;

        public async Task<T> GetAsync<T>(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url), "URL cannot be null or empty");
            }

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Request Hata: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Belirsiz Hata: {ex.Message}");
                throw;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task<T> GetByElment<T>(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url), "Hata");
            }

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("Hata.", httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Hata", ex);
            }

            var json = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonException jsonEx)
            {

                throw new Exception("Hata.", jsonEx);
            }
        }
    }
}
