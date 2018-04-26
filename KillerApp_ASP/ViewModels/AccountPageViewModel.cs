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

        [Range(0, 1000)]
        public float Fund { get; set; }

        public List<OrderTrack> Tracks { get; set; }
        
        public AccountPageViewModel()
        {
            
        }

        public AccountPageViewModel(int id, float fund, List<OrderTrack> tracks)
        {
            Id = id;
            Fund = fund;
            Tracks = tracks;
        }
    }
}