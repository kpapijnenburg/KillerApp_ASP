using KillerAppClassLibrary.Classes;
using System.Collections.Generic;

namespace KillerApp_ASP.ViewModels
{
    public class AsyncTrackViewModel
    {
        public AsyncTrackViewModel(Track track, string base64)
        {
            Track = track;
            Base64 = base64;
        }

        public Track Track { get; set; }
        public string Base64 { get; set; }

    
    }
}