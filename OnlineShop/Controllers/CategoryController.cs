using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Contracts;
using OnlineShop.DataModels;
using OnlineShop.DTOs.Category;
using OnlineShop.DTOs.Roles;

namespace OnlineShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var Category = await _categoryRepository.CreateAsync(createCategoryDto);
            return Ok(Category);
        }

        [HttpGet("Get Category")]
        public async Task<ActionResult<GetCategoryDto>> GetCategory(int categoryId)
        {
            var category = await this._categoryRepository.GetAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"CategoryId{categoryId} is not Found");
            }
            var categoryDetailsDto = _mapper.Map<GetCategoryDto>(category);
            return Ok(categoryDetailsDto);
        }

        [HttpGet("Get All Categories")]
        public async Task<ActionResult<List<GetCategoryDto>>> GetAllCategories()
        {
            var categories = await this._categoryRepository.GetAllAsync();
            var records = _mapper.Map<List<GetCategoryDto>>(categories);
            return Ok(records);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            var category = await _categoryRepository.UpdateAsync(updateCategoryDto);
            return Ok(category);
        }

        [HttpDelete("Delete Category")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            await _categoryRepository.DeleteAsync(categoryId);
            return NoContent();
        }
    }
}
