using System;
using System.Linq;
using System.Web.Mvc;
using KillerAppClassLibrary.Classes;
using KillerAppClassLibrary.Context.Sql;
using KillerAppClassLibrary.Logic.Repositories;
using KillerApp_ASP.ViewModels;
using PagedList;
using Controller = System.Web.Mvc.Controller;

namespace KillerApp_ASP.Controllers
{
    public class TrackController : Controller
    {
        private readonly TrackRepository repository;
        private readonly VoteRepository voteRepository;
        private readonly UserRepository userRepository;
        private readonly CommentRepository commentRepository;

        public TrackController()
        {
            voteRepository = new VoteRepository(new VoteSqlContext());
            repository = new TrackRepository(new TrackSqlContext());
            userRepository = new UserRepository(new UserSqlContext());
            commentRepository = new CommentRepository(new CommentSqlContext());
        }

        [HttpGet]
        public ActionResult Details(int id, int? page)
        {
            RedirectToRouteResult result;

            var cookie = Request.Cookies["userId"];

            if (cookie == null)
            {
                result = RedirectToAction("LoginForm", "User");
            }
            else
            {
                var user = userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));
                var track = repository.GetById(id);
                var comments = commentRepository.GetByTrackId(id).ToList();
                comments.Reverse();

                int pageNumber = page ?? 1;
                var onePageOfComments = comments.ToPagedList(pageNumber, 3);

                ViewBag.Items = onePageOfComments;

                var model = new TrackDetailsViewModel
                (
                    new TrackViewModel
                    (
                        track.Id,
                        track.ArtistName,
                        track.TrackName,
                        track.Label,
                        track.Price,
                        track.Cover,
                        track.Deal
                    ),
                    voteRepository.GetVote(track, user),
                    user,
                    voteRepository.HasVoted(track, user)
                );

                return View(model);
            }

            return result;
        }

        [HttpGet]
        public ActionResult EditForm(int id)
        {
            var track = repository.GetById(id);

            var model = new TrackViewModel
            (
                track.Id,
                track.ArtistName,
                track.TrackName,
                track.Label,
                track.Price,
                track.Cover,
                track.Deal
            );

            return View(model);
        }

        public ActionResult Remove(int id)
        {
            repository.Delete(id);

            return RedirectToAction("Adminpage", "User");
        }

        [HttpPost]
        public ActionResult Edit(TrackViewModel model)
        {
            var track = new Track
            (
                model.Id,
                model.ArtistName,
                model.TrackName,
                model.Label,
                model.Price,
                model.Deal,
                model.Cover
            );

            track.Price = track.Deal ? 0.59m : 0.99m;

            repository.Update(track);

            return RedirectToAction("Adminpage", "User");

        }

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TrackViewModel model)
        {
            RedirectToRouteResult result;

            if (!ModelState.IsValid)
            {
                result = RedirectToAction("New", "Track");
            }
            else
            {
                var track = new Track
                (
                    model.ArtistName,
                    model.TrackName,
                    model.Label,
                    model.Price,
                    model.Deal,
                    model.Cover
                );

                repository.Create(track);

                result = RedirectToAction("Adminpage", "User");
            }

            return result;
        }

        [HttpGet]
        public ActionResult GetLatestReleases()
        {
            return Json(repository.GetLatestReleases(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMostDownloaded()
        {
            return Json(repository.GetMostDownloadedTracks(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDeals()
        {
            var tracks = repository.GetAll().Where(t => t.Deal).ToList();

            if (!tracks.Any()) return null;
            var random = new Random();
            int r = random.Next(tracks.Count);

            var track = tracks[r];

            var model = new AsyncTrackViewModel
                (
                track,
                Convert.ToBase64String(track.Cover)
                );

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRecommended()
        {
            var cookie = Request.Cookies["userId"];

            Track track;

            if (cookie == null)
            {
                var random = new Random();

                var tracks = repository.GetAll().Take(10).ToList();
                track = tracks[random.Next(10)];

            }
            else
            {
                var user = userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));

                track = repository.GetRecommended(user);

            }

            var model = new AsyncTrackViewModel
                (
                track, 
                Convert.ToBase64String(track.Cover
                ));


            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }

}