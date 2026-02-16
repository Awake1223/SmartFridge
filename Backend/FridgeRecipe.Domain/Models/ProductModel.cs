using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeRecipe.Domain.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime AddedDate { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public bool IsInFridge { get; set; }

    }
}
