
using CreatedMeetWebUI.ApiJobs;
using System.Web;

namespace CreatedMeetWebUI.Tools.User
{
    public class UserService<T> : IUserService<T> where T : class, new()
    {
        private readonly IApiService _apiService;

        public UserService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<T> ValidateUserAsync(string username, string password)
        {
            var apiUrl = "https://localhost:44359/User/UserControlIdentity";
            var encodedUsername = HttpUtility.UrlEncode(username);
            var encodedPassword = HttpUtility.UrlEncode(password);
            var fullUrl = $"{apiUrl}?USERNAME={encodedUsername}&PASSWORD={encodedPassword}";

            var apiLoads = ApiLoads<T>.GetInstance(fullUrl);
            var controlUser = new T(); // Generik model için bir örnek oluştur
            var loadTask = apiLoads.LoadListParaMetresAsync(controlUser, fullUrl);
            await loadTask; // Asenkron işlemi bekle

            var result = await loadTask; // Yüklenen değeri al
            var userProperty = typeof(T).GetProperty("ID");

            if (userProperty != null && (int)userProperty.GetValue(result) != 0)
                return result;
            else
                return null;
        }

        // Örnekleme için kullanılacak basit kullanıcı listesi (gerçek uygulama için veritabanı yerine kullanılmamalıdır)
        private static readonly List<(string Username, string Password)> Users = new List<(string Username, string Password)>
        {
            // Örnek kullanıcılar
        };

        public Task<bool> CreateUserAsync(string email, string password)
        {
            // Kullanıcıyı basit bir listeye ekler (gerçek uygulama için veritabanına ekleme yapılmalıdır)
            Users.Add((email, password));
            return Task.FromResult(true);
        }

        public Task<bool> SignInUserAsync(string email, string password)
        {
            // Kullanıcı doğrulama işlemi yapar
            bool isValid = Users.Any(user => user.Username == email && user.Password == password);
            return Task.FromResult(isValid);
        }
    }
}
