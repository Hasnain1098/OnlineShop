using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Contracts;
using OnlineShop.DataModels;

namespace OnlineShop.Repository
{
    public class RoleRepository : IRoleRepository
    {


        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            _roleManager = roleManager;
        }
        public async Task<Role> CreateAsync(Role role)
        {
            var roleStore = new RoleStore<IdentityRole>(_context); //Pass the instance of your DbContext here

            if (!await _roleManager.RoleExistsAsync(role.Name))
                await _roleManager.CreateAsync(new IdentityRole(role.Name));

            await this._context.AddAsync(role);
            await this._context.SaveChangesAsync();
            return role;
        }

        public async Task DeleteAsync(int roleId)
        {
            var role = await GetAsync(roleId);

            if (role is null)
            {
                throw new Exception($"roleID {roleId} is not found.");
            }
            this._context.Set<Role>().Remove(role);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Set<Role>().ToListAsync();
        }

        public async Task<Role?> GetAsync(int? roleId)
        {
            if (roleId == null)
            {
                return null;
            }
            return await this._context.Roles.FindAsync(roleId);
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Update(role);
            await _context.SaveChangesAsync();
        }
    }
}
