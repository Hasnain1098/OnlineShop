using Microsoft.EntityFrameworkCore;
using OnlineShop.Contracts;
using OnlineShop.DataModels;

namespace OnlineShop.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public CategoryRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            //var roleStore = new RoleStore<IdentityRole>(_context); //Pass the instance of your DbContext here

            //if (!await _roleManager.RoleExistsAsync(role.Name))
            //    await _roleManager.CreateAsync(new IdentityRole(role.Name));

            await this._context.AddAsync(category);
            await this._context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(int categoryId)
        {
            var category = await GetAsync(categoryId);

            if (category is null)
            {
                throw new Exception($"categoryID {categoryId} is not found.");
            }
            this._context.Set<Category>().Remove(category);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Set<Category>().ToListAsync();
        }

        public async Task<Category?> GetAsync(int? categoryId)
        {
            if (categoryId == null)
            {
                return null;
            }
            return await this._context.Categories.FindAsync(categoryId);
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
