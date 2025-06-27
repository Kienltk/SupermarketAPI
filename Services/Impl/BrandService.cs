using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;
using SupermarketAPI.Repositories.Impl;

namespace SupermarketAPI.Services.Impl
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public async Task<List<BrandDto>> GetBrands()
        {
            var brands = await _brandRepository.GetBrandsAsync();

            return brands.Select(b => MapToBrandDTO(b)).ToList();
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
