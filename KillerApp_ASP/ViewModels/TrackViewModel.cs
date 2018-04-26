using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KillerApp_ASP.ViewModels
{
    public class TrackViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Artists name")]
        public string ArtistName { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Track name")]
        public string TrackName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Label { get; set; }

        [Required]
        [Range(1,10)]
        public float Price { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Cover")]
        [Url]
        public string CoverUrl { get; set; }

        public TrackViewModel(string artistName, string trackName, string label, float price, string coverUrl)
        {
            ArtistName = artistName;
            TrackName = trackName;
            Label = label;
            Price = price;
            CoverUrl = coverUrl;
        }

        public TrackViewModel(int id, string artistName, string trackName, string label, float price, string coverUrl)
        {
            Id = id;
            ArtistName = artistName;
            TrackName = trackName;
            Label = label;
            Price = price;
            CoverUrl = coverUrl;
        }

        public TrackViewModel()
        {

        }
    }
}