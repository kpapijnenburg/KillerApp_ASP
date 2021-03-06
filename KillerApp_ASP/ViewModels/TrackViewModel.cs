﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using KillerApp_ASP.Models;

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
        [Range(0, 10)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Cover")]
        public byte[] Cover { get; set; }

        public bool Deal { get; set; }

        

        public TrackViewModel(string artistName, string trackName, string label, decimal price, byte[] cover, bool deal)
        {
            ArtistName = artistName;
            TrackName = trackName;
            Label = label;
            Price = price;
            Cover = cover;
            Deal = deal;
        }

        public TrackViewModel(int id, string artistName, string trackName, string label, decimal price, byte[] cover, bool deal)
        {
            Id = id;
            ArtistName = artistName;
            TrackName = trackName;
            Label = label;
            Price = price;
            Cover = cover;
            Deal = deal;
        }

        public TrackViewModel()
        {

        }
    }
}