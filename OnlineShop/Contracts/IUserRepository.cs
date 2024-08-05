using OnlineShop.DataModels;
using OnlineShop.Repository;

namespace OnlineShop.Contracts
{
    public interface IUserRepository
    {
        Task<(int, string)> CreateUser(RegistrationModel model,  AllRoles roles);
        Task<(int, string)> Login(LoginModel model);

        Task<(int, string)> DeleteUserAsync(string emailId);

        Task<(int, string)> UpdatePasswordAsync(RegistrationModel model, string newPassword);

        Task<User?> GetAsync(string? userId);

        Task<List<User>> GetAllAsync();
    }
}
