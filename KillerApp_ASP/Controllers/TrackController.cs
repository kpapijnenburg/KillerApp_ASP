using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KillerAppClassLibrary.Classes;
using KillerAppClassLibrary.Context.Interface;
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

        public ActionResult Details(int id, int? page)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound();
            }

            var cookie = Request.Cookies["userId"];

            if (cookie == null)
            {
                return RedirectToAction("LoginForm", "User");
            }

            var user = userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));
            var track = repository.GetById(id);
            var comments = commentRepository.GetByTrackId(id).ToList();

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
                        track.CoverUrl
                    ),
                    voteRepository.GetVote(track, user),
                    user,
                    voteRepository.HasVoted(track, user)
                );

            return View(model);
        }


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
                track.CoverUrl
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
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Adminpage", "User");
            }

            var track = new Track
                (
                    model.Id,
                    model.ArtistName,
                    model.TrackName,
                    model.Label,
                    model.Price,
                    model.CoverUrl
                );

            repository.Update(track);

            return RedirectToAction("Adminpage", "User");

        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Create(TrackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("New", "Track");
            }

            var track = new Track
                (
                    model.ArtistName,
                    model.TrackName,
                    model.Label,
                    model.Price,
                    model.CoverUrl
                );

            repository.Create(track);

            return RedirectToAction("Adminpage", "User");
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
    }
}