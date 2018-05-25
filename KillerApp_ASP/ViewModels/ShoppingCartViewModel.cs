using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KillerAppClassLibrary.Classes;

namespace KillerApp_ASP.ViewModels
{
    public class ShoppingCartViewModel
    {
        public User User { get; set; }
        public decimal TotalPrice { get; set; }

        public ShoppingCartViewModel()
        {
            
        }

        public ShoppingCartViewModel(User user, decimal totalPrice)
        {
            User = user;
            TotalPrice = totalPrice;
        }
    }
}