using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeRecipe.Domain.Models
{
    public class ShoppingItemModel
    {
        public Guid Id { get; set; }

        public Guid UserId  { get; set; }
        public UserModel User { get; set; }

        public Guid ProductId { get; set; }
        public ProductModel Product {  get; set; }

        public double Quantity { get; set; } //КОЛИЧЕСТВО
        public string Unit {get; set; } //Единицы измерения

        public bool IsBought { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
