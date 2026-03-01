using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoAppMobile.Models;
public class ProductItem
{
    public string Name { get; set; }
    public string Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string ImageUrl { get; set; }
    public int FreshnessLevel { get; set; } // 1-7, где 1 - самый свежий, 7 - испорчен
}
