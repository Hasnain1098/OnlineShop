using OnlineShop.DataModels;
using OnlineShop.DTOs.Category;

namespace OnlineShop.Contracts
{
    public interface ICategoryRepository
    {
        Task<GetCategoryDto?> GetAsync(int? categoryId);

        Task<IEnumerable<GetCategoryDto>> GetAllAsync();

        Task<Category> CreateAsync(CreateCategoryDto createCategoryDto);

        Task DeleteAsync(int categoryId);

        Task<Category?> UpdateAsync(UpdateCategoryDto updateCategoryDTO);
    }
}
