using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace KillerApp_ASP.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [EmailAddress]
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Range(0,1000)]
        public float Fund { get; set; }

        public bool IsAdmin { get; set; }

        public UserViewModel(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            Fund = 0;
            IsAdmin = false;
        }

        public UserViewModel(int id, string username, string email, string password, float fund)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            Fund = fund;
            IsAdmin = false;
        }

        public UserViewModel()
        {

        }
    }
}