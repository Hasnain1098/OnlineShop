using OnlineShop.DataModels;

namespace OnlineShop.Contracts
{
    public interface IRoleRepository
    {
        Task<Role?> GetAsync(int? roleId);

        Task<List<Role>> GetAllAsync();

        Task<Role> CreateAsync(Role role);

        Task DeleteAsync(int roleId);

        Task UpdateAsync(Role role);
    }
}
