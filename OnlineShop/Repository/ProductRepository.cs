using Microsoft.EntityFrameworkCore;
using OnlineShop.Contracts;
using OnlineShop.DataModels;

namespace OnlineShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public ProductRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<Product> CreateAsync(Product product)
        {
            //var roleStore = new RoleStore<IdentityRole>(_context); //Pass the instance of your DbContext here

            //if (!await _roleManager.RoleExistsAsync(role.Name))
            //    await _roleManager.CreateAsync(new IdentityRole(role.Name));

            await this._context.AddAsync(product);
            await this._context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int productId)
        {
            var product = await GetAsync(productId);

            if (product is null)
            {
                throw new Exception($"productID {productId} is not found.");
            }
            this._context.Set<Product>().Remove(product);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Set<Product>().ToListAsync();
        }

        public async Task<Product?> GetAsync(int? productId)
        {
            if (productId == null)
            {
                return null;
            }
            return await this._context.Products.FindAsync(productId);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}

