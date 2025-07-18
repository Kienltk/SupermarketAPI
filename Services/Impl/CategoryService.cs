using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly SupermarketContext _context;

        public CategoryService(ICategoryRepository categoryRepository, SupermarketContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task<List<CategoriesDto>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return MapToCategoryDTOs(categories);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto dto)
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == dto.CategoryName || c.Slug == dto.slug);

            if (existingCategory != null)
            {
                throw new Exception("Category name or slug already exists.");
            }

            var category = new Category
            {
                CategoryName = dto.CategoryName,
                Slug = dto.slug,
                ParentId = dto.ParentId
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            dto.Id = category.CategoryId;
            return dto;
        }
        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new Exception("Category not found");

            return new CategoryDto
            {
                Id = category.CategoryId,
                CategoryName = category.CategoryName,
                slug = category.Slug,
                ParentId = category.ParentId,
                 
            };
        }
        public async Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new Exception("Category not found");

            category.CategoryName = dto.CategoryName;
            category.Slug = dto.slug;
            category.ParentId = dto.ParentId;
            await _context.SaveChangesAsync();

            return dto;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new Exception("Category not found");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        private List<CategoriesDto> MapToCategoryDTOs(List<Category> categories)
        {
            return categories.Select(c => new CategoriesDto
            {
                CategoryDto = new CategoryDto
                {
                    Id = c.CategoryId,
                    CategoryName = c.CategoryName,
                    slug = c.Slug,
                    ParentId = c.ParentId
                },
                Children = MapToCategoryDTOs(c.InverseParent.ToList())
            }).ToList();
        }
    }
}
