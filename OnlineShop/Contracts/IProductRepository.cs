using OnlineShop.DataModels;

namespace OnlineShop.Contracts
{
    public interface IProductRepository
    {
        Task<Product?> GetAsync(int? productId);

        Task<List<Product>> GetAllAsync();

        Task<Product> CreateAsync(Product product);

        Task DeleteAsync(int productId);

        Task UpdateAsync(Product product);
    }
}
