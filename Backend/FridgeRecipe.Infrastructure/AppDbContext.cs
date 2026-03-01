using FridgeRecipe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FridgeRecipe.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ShoppingItemModel> ShoppingItems { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
