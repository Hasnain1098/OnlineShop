using OnlineShop.DataModels;

namespace OnlineShop.Contracts
{
    public interface IAuthService
    {
        Task<(int, string)> Registeration(RegistrationModel model, string role);
        Task<(int, string)> Login(LoginModel model);
    }
}
