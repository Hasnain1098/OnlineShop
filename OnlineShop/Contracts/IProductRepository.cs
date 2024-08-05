using OnlineShop.DataModels;
using OnlineShop.DTOs.Product;

namespace OnlineShop.Contracts
{
    public interface IProductRepository
    {
        Task<Product?> CreateAsync(CreateProductDto createProductDTO);

        Task<GetProductDto?> GetAsync(int? productId);
       

        Task<List<GetProductDto>> GetAllAsync();


        Task DeleteAsync(int id);

        Task<Product?> UpdateAsync(UpdateProductDto updateProductDTO);
    }
}
