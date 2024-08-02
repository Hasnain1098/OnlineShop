using OnlineShop.DataModels;

namespace OnlineShop.Contracts
{
    public interface IUserRepository
    {
        Task<(int, string)> CreateUser(RegistrationModel model, string role);
        Task<(int, string)> Login(LoginModel model);

        Task<(int, string)> DeleteUserAsync(string emailId);

        Task<(int, string)> UpdatePasswordAsync(RegistrationModel model, string newPassword);

        Task<User?> GetAsync(string? userId);

        Task<List<User>> GetAllAsync();
    }
}
