using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using KillerAppClassLibrary.Classes;

namespace KillerApp_ASP.ViewModels
{
    public class AccountPageViewModel
    {
        public int Id { get; set; }

        [Range(0, 10)]
        public decimal Fund { get; set; }
        
        public AccountPageViewModel()
        {
            
        }

        public AccountPageViewModel(int id, decimal fund)
        {
            Id = id;
            Fund = fund;
        }
    }
}