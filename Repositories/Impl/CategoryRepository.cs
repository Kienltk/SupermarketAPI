using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories.Impl
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SupermarketContext _context;

        public CategoryRepository(SupermarketContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryBySlugAsync(string slug)
        {
            return await _context.Categories.FirstAsync(c => c.Slug == slug);
        }

        public async Task<List<Category>> GetCategoriesByParentIdAsync(int? parentId)
        {
            return await _context.Categories
                .Where(c => c.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var allCategories = await _context.Categories.ToListAsync();

            var lookup = allCategories.ToLookup(c => c.ParentId);
            foreach (var category in allCategories)
            {
                category.InverseParent = lookup[category.CategoryId].ToList();
            }

            return lookup[null].ToList();
        }

        public int GetCategoryIdByProductId(int productId)
        {
            return _context.ProductCategories
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.CategoryId)
                .FirstOrDefault();
        }

        public Category GetParentCategoryByCategoryId(int categoryId)
        {
            var category = _context.Categories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.ParentId.HasValue
                    ? _context.Categories.FirstOrDefault(pc => pc.CategoryId == c.ParentId)
                    : c)
                .FirstOrDefault() ?? throw new InvalidOperationException($"Parent category for ID {categoryId} not found.");

            return category;
        }
    }
}
