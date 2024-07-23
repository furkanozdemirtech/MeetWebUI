namespace CreatedMeetWebUI.Tools.User
{
    public interface IUserService<T>
    {
        Task<T> ValidateUserAsync(string username, string password);

        // Kullanıcı oluşturma işlemi, e-posta ve şifre alır ve işlemin başarılı olup olmadığını döndürür
        Task<bool> CreateUserAsync(string email, string password);

        // Kullanıcı giriş işlemi, e-posta ve şifre alır ve işlemin başarılı olup olmadığını döndürür
        Task<bool> SignInUserAsync(string email, string password);
    }
}
