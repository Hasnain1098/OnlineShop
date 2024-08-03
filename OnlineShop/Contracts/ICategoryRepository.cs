using OnlineShop.DataModels;

namespace OnlineShop.Contracts
{
    public interface ICategoryRepository
    {
        Task<Category?> GetAsync(int? categoryId);

        Task<List<Category>> GetAllAsync();

        Task<Category> CreateAsync(Category category);

        Task DeleteAsync(int categoryId);

        Task UpdateAsync(Category category);
    }
}
