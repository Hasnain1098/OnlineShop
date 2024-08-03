using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Contracts;
using OnlineShop.DataModels;
using OnlineShop.DTOs.Category;
using OnlineShop.DTOs.Product;

namespace OnlineShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public ProductController(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [HttpPost("Add Product")]
        public async Task<ActionResult<Product>> CreateRole(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            await this._productRepository.CreateAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpGet("Get product")]
        public async Task<ActionResult<GetProductDto>> GetProduct(int productId)
        {
            var product = await this._productRepository.GetAsync(productId);
            if (product == null)
            {
                throw new Exception($"ProductId{productId} is not Found");
            }
            var productDetailsDto = _mapper.Map<GetProductDto>(product);
            return Ok(productDetailsDto);
        }

        [HttpGet("Get All Products")]
        public async Task<ActionResult<List<GetProductDto>>> GetAllProducts()
        {
            var products = await this._productRepository.GetAllAsync();
            var records = _mapper.Map<List<GetProductDto>>(products);
            return Ok(records);
        }

        [HttpPut("Update Products")]
        public async Task<ActionResult> UpdateProducts(int productId, UpdateProductDto updateProductDto)
        {
            if (productId!= updateProductDto.Id)
            {
                return BadRequest("Invalid PRODUCT Id: ");
            }

            var product = await _productRepository.GetAsync(productId);
            if (product == null)
            {
                throw new Exception($"ProductID {productId} is not found");
            }

            _mapper.Map(updateProductDto, product);

            try
            {
                await _productRepository.UpdateAsync(product);
            }
            catch (Exception)
            {
                throw new Exception($"Error occured while updating productID {productId}.");
            }

            return NoContent();
        }

        [HttpDelete("Delete Product")]
        public async Task<IActionResult> DeleteCategory(int productId)
        {
            await _productRepository.DeleteAsync(productId);
            return NoContent();
        }
    }
}
