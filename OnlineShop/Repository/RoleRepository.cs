using OnlineShop.Contracts;
using OnlineShop.DataModels;

namespace OnlineShop.Repository
{
    public class RoleRepository : IRoleRepository
    {
        public Task<Role> CreateAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Role>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Role?> GetAsync(int? roleId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
