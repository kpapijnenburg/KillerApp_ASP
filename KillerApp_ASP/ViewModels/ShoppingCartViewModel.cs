using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KillerAppClassLibrary.Classes;

namespace KillerApp_ASP.ViewModels
{
    public class ShoppingCartViewModel
    {
        public UserViewModel UserViewModel { get; set; }
        public List<Track> ShoppingCart { get; set; }

        public ShoppingCartViewModel(UserViewModel userViewModel, List<Track> shoppingCart)
        {
            UserViewModel = userViewModel;
            ShoppingCart = shoppingCart;
        }

        public ShoppingCartViewModel()
        {

        }

    }
}