using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeRecipe.Domain.Models
{
    public class UserModel
    {
        public Guid Id {  get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string SecondName {  get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        //  public static string HashPassword(string password);


        public ICollection<ShoppingItemModel> ShoppingItems { get; set; } = new List<ShoppingItemModel>();
    }
}
