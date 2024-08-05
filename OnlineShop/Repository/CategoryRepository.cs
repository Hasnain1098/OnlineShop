using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Contracts;
using OnlineShop.DataModels;
using OnlineShop.DTOs.Category;

namespace OnlineShop.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        //private readonly RoleManager<IdentityRole> _roleManager;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<Category> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            //var roleStore = new RoleStore<IdentityRole>(_context); //Pass the instance of your DbContext here

            //if (!await _roleManager.RoleExistsAsync(role.Name))
            //    await _roleManager.CreateAsync(new IdentityRole(role.Name));

            Category category = _mapper.Map<Category>(createCategoryDto);
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(int categoryId)
        {
            Category? category = null;
            category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(); // Ensure changes are saved
            }
            else
            {
                throw new NullReferenceException($"category not found");
            }
        }

        public async Task<IEnumerable<GetCategoryDto>> GetAllAsync()
        {
            var categories = await _context.Categories
                                           .Include(c => c.Products)
                                           .ToListAsync();
            return _mapper.Map<IEnumerable<GetCategoryDto>>(categories);
        }

        public async Task<GetCategoryDto?> GetAsync(int? categoryId)
        {
            var category = await _context.Categories
                                         .Include(c => c.Products)
                                         .FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category != null)
                return _mapper.Map<GetCategoryDto>(category);
            else
                throw new NullReferenceException($"category not found");
        }

        public async Task<Category?> UpdateAsync(UpdateCategoryDto updateCategoryDTO)
        {
            var category = await _context.Categories.FindAsync(updateCategoryDTO.CategoryID);
            if (category is null)
                throw new NullReferenceException("category not found");
            category.Name = updateCategoryDTO.NewName;
            category.Description = updateCategoryDTO.NewDescription;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
