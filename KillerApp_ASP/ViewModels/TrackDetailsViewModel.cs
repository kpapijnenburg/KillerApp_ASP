using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KillerAppClassLibrary.Classes;

namespace KillerApp_ASP.ViewModels
{
    public class TrackDetailsViewModel
    {
        public TrackViewModel TrackViewModel { get; set; }
        public Vote Vote { get; set; }
        public User User { get; set; }
        public bool HasVoted { get; set; }

        public TrackDetailsViewModel()
        {
            
        }

        public TrackDetailsViewModel(TrackViewModel trackViewModel, Vote vote,User user, bool hasVoted)
        {
            TrackViewModel = trackViewModel;
            Vote = vote;
            User = user;
            HasVoted = hasVoted;
        }
    }
}