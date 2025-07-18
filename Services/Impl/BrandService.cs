// BrandService.cs
using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services.Impl
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly SupermarketContext _context;

        public BrandService(IBrandRepository brandRepository, SupermarketContext context)
        {
            _brandRepository = brandRepository;
            _context = context;
        }

        public async Task<BrandDto> CreateBrandAsync(BrandDto dto)
        {
            var existingBrand = await _context.Brands
                .FirstOrDefaultAsync(b => b.BrandName == dto.BrandName || b.Slug == dto.Slug);

            if (existingBrand != null)
            {
                throw new Exception("Brand name or slug already exists.");
            }

            var brand = new Brand
            {
                BrandName = dto.BrandName,
                Slug = dto.Slug,
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            dto.Id = brand.BrandId;
            return dto;
        }

        public async Task DeleteBrandAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                throw new Exception("Brand not found");

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<BrandDto> GetBrandByIdAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                throw new Exception("Brand not found");

            return MapToBrandDTO(brand);
        }

        public async Task<List<BrandDto>> GetBrands()
        {
            var brands = await _brandRepository.GetBrandsAsync();
            return brands.Select(b => MapToBrandDTO(b)).ToList();
        }

        public async Task<BrandDto> UpdateBrandAsync(int id, BrandDto dto)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                throw new Exception("Brand not found");

            brand.BrandName = dto.BrandName;
            brand.Slug = dto.Slug;
            await _context.SaveChangesAsync();

            return dto;
        }

        private BrandDto MapToBrandDTO(Brand brand)
        {
            return new BrandDto
            {
                Id = brand.BrandId,
                BrandName = brand.BrandName,
                Slug = brand.Slug,
            };
        }
    }
}
