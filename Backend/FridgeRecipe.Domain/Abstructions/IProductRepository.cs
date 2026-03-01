using FridgeRecipe.Domain.Models;

namespace FridgeRecipe.Infrastructure.Repository
{
    public interface IProductRepository
    {
        Task AddAsync(ProductModel product);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ProductModel>> GetAllAsync();
        Task<ProductModel?> GetByIdAsync(Guid id);
        Task UpdateAsync(ProductModel product);
    }
}