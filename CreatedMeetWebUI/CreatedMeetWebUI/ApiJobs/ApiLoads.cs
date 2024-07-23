using Newtonsoft.Json;
using System.Text;

namespace CreatedMeetWebUI.ApiJobs
{
    public class ApiLoads<T>
    {
        private static ApiLoads<T> instance;
        private static readonly object padlock = new object();

        private readonly IApiService _apiService;
        private readonly string _apiUrl;

        public List<T> List { get; private set; }
        public List<T> SecondaryList { get; private set; }

        private ApiLoads(IApiService apiService, string apiUrl)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _apiUrl = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));
        }

        public static ApiLoads<T> GetInstance(string apiUrl)
        {
            if (string.IsNullOrEmpty(apiUrl))
            {
                throw new ArgumentNullException(nameof(apiUrl), "API URL cannot be null or empty");
            }

            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ApiLoads<T>(ApiService.Instance, apiUrl);
                    }
                }
            }
            return instance;
        }

        public async Task LoadtListAsync()
        {
            try
            {
                List = await _apiService.GetAsync<List<T>>(_apiUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading  list: {ex.Message}");
            }
        }
        public async Task<HttpResponseMessage> PostAsync(string url, T model)
        {
            using (var client = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                return response;
            }
        }
        public async Task<T> LoadListParaMetresAsync(T model, string url)
        {
            try
            {

                var user = await _apiService.GetByElment<T>(url);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Hata.", ex);
            }
        }
        //ToQueryString metodo model nesnelerinin daha rahat bir şekilde  veri setinden parametre alınması için burada birakıyoruz
        private string ToQueryString<U>(U parameters)
        {
            if (parameters == null)
            {
                return string.Empty;
            }

            var properties = from p in typeof(U).GetProperties()
                             let value = p.GetValue(parameters, null)
                             where value != null &&
                                   !(value is int intValue && intValue == 0) &&
                                   !(value is double doubleValue && doubleValue == 0.0) &&
                                   !(value is decimal decimalValue && decimalValue == 0.0m) &&
                                   !(value is float floatValue && floatValue == 0.0f) &&
                                   !(value is bool boolValue && (boolValue == false || boolValue == true))
                             select $"{p.Name}={Uri.EscapeDataString(value.ToString())}";

            return string.Join("&", properties.ToArray());
        }
    }
}
