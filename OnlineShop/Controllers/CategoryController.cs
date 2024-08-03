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

        [HttpPost("Add Category")]
        public async Task<ActionResult<Category>> CreateRole(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            await this._categoryRepository.CreateAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
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

        [HttpPut("Update Categories")]
        public async Task<ActionResult> UpdateRoles(int categoryId, UpdateCategoryDto updateCategoryDto)
        {
            if (categoryId!= updateCategoryDto.Id)
            {
                return BadRequest("Invalid Category Id");
            }

            var category = await _categoryRepository.GetAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"CategoryID {categoryId} is not found");
            }

            _mapper.Map(updateCategoryDto, category);

            try
            {
                await _categoryRepository.UpdateAsync(category);
            }
            catch (Exception)
            {
                throw new Exception($"Error occured while updating categoryID {categoryId}.");
            }

            return NoContent();
        }

        [HttpDelete("Delete Category")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            await _categoryRepository.DeleteAsync(categoryId);
            return NoContent();
        }
    }
}
