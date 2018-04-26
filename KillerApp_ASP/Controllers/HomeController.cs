using System.Linq;
using System.Web.Mvc;
using KillerAppClassLibrary.Context.Sql;
using KillerAppClassLibrary.Logic.Repositories;
using KillerApp_ASP.ViewModels;
using PagedList;
using Controller = System.Web.Mvc.Controller;


namespace KillerApp_ASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly TrackRepository trackRepository;

        public HomeController()
        {
            trackRepository = new TrackRepository(new TrackSqlContext());
        }

        public ActionResult Index(int? page, string search, string options)
        {
            var tracks = trackRepository.GetAll();
            
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(options))
            {
                tracks = options == "artist" 
                    ? tracks.Where(t => t.ArtistName.ToLower().Contains(search.ToLower())) 
                    : tracks.Where(t => t.TrackName.ToLower().Contains(search.ToLower()));
            }

            int pageNumber = page ?? 1;
            var onePageOfTracks = tracks.ToPagedList(pageNumber, 9);

            ViewBag.Items = onePageOfTracks;

            return View();
        }
    }
}