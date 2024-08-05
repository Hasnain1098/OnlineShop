using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineShop.Contracts;
using OnlineShop.DataModels;
using OnlineShop.DTOs.Product;
using System.Runtime.CompilerServices;

namespace OnlineShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private const string cacheKey = "productDetailsList";
        //private readonly RoleManager<IdentityRole> _roleManager;

        public ProductRepository(ApplicationDbContext context, IMapper mapper, IMemoryCache memoryCache)
        {
            this._context = context;
            this._mapper = mapper;
            this._memoryCache = memoryCache;
        }
        public async Task<Product?> CreateAsync(CreateProductDto createProductDTO)
        {
            Product product = _mapper.Map<Product>(createProductDTO);
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            InvalidateCache();
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            Product? product = null;
            product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                InvalidateCache();
                _context.Products.Remove(product);
                await _context.SaveChangesAsync(); // Ensure changes are saved
            }
            else
            {
                throw new NullReferenceException($"product not found");
            }
        }

        public async Task<List<GetProductDto>> GetAllAsync()
        {
            
            if (_memoryCache.TryGetValue(cacheKey, out List<GetProductDto> cachedProductDetailsList))
            {
                return cachedProductDetailsList;
            }

            // Retrieve products and include categories
            List<Product> products = await _context.Products.Include(p => p.Category).ToListAsync();

            // Map products to DTOs
            List<GetProductDto> productDetailsList = _mapper.Map<List<GetProductDto>>(products);

            // Set category names in DTOs
            foreach (var productDetails in productDetailsList)
            {
                Category? category = await _context.Categories.FindAsync(productDetails.CategoryId);
                productDetails.CategoryName = category?.Name ?? throw new NullReferenceException($"Invalid category for product ID: {productDetails.Id}");
            }

            // Cache the result
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(100), // Cache for 100 minutes
                SlidingExpiration = TimeSpan.FromMinutes(100) // Expire if not accessed for 100 minutes
            };
            _memoryCache.Set(cacheKey, productDetailsList, cacheOptions);

            return productDetailsList;
        }

        public async Task<GetProductDto?> GetAsync(int? productId)
        {
            Product? product = await _context.Products.FindAsync(productId);
            GetProductDto? productDetails = _mapper.Map<GetProductDto?>(product);

            Category? category = await _context.Categories.FindAsync(productDetails.CategoryId);

            productDetails.CategoryName = (category is null) ? throw new NullReferenceException($"Invalid category") : category.Name;

            if (product is not null)
                return productDetails;
            else
                throw new NullReferenceException($"product not found");
        }

        public async Task<Product?> UpdateAsync(UpdateProductDto updateProductDTO)
        {
            var product = await _context.Products.FindAsync(updateProductDTO.ProductID);
            if (product is null)
                throw new NullReferenceException("product not found");
            product.Name = updateProductDTO.NewName;
            product.Description = updateProductDTO.NewDescription;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            InvalidateCache();
            return product;
        }
        private void InvalidateCache() => _memoryCache.Remove(cacheKey);

    }
}

