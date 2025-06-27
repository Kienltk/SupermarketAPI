using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoriesDto>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return MapToCategoryDTOs(categories);
        }

        private List<CategoriesDto> MapToCategoryDTOs(List<Category> categories)
        {
            return categories.Select(c => new CategoriesDto
            {
                Id = c.CategoryId,
                CategoryName = c.CategoryName,
                slug = c.Slug,
                Children = MapToCategoryDTOs(c.InverseParent.ToList())
            }).ToList();
        }
    }
}