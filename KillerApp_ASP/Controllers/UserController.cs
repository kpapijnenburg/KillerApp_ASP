using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KillerAppClassLibrary.Classes;
using KillerAppClassLibrary.Context.Interface;
using KillerAppClassLibrary.Context.Sql;
using KillerAppClassLibrary.Logic.Repositories;
using KillerApp_ASP.Models;
using KillerApp_ASP.ViewModels;
using PagedList;
using Controller = System.Web.Mvc.Controller;


namespace KillerApp_ASP.Controllers
{
    public class UserController : Controller
    {

        private readonly UserRepository userRepository = new UserRepository(new UserSqlContext());
        private readonly TrackRepository trackRepository = new TrackRepository(new TrackSqlContext());

        public ActionResult LoginForm()
        {
            return View(new UserViewModel());
        }

        public ActionResult RegisterForm()
        {
            return View();
        }

        public ActionResult Register(UserViewModel model)
        {
            var user = new User
                (
                model.Username,
                model.Email,
                model.Password
                );

            try
            {
                userRepository.Create(user);

                return RedirectToAction("index", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RedirectToAction("RegisterForm", "User");
            }

            return RedirectToAction("index", "Home");
        }

        [HttpPost]
        public ActionResult Login(UserViewModel model)
        {
            try
            {
                var user = userRepository.Login(model.Email, model.Password);

                var cookie = new HttpCookie("userId");
                cookie.Values.Add("userId", user.Id.ToString());
                cookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(cookie);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                RedirectToAction("LoginForm", "User");
            }

            return RedirectToAction("index", "Home");
        }

        public ActionResult Logout()
        {
            var cookie = Request.Cookies["userId"];
            if (cookie == null) return RedirectToAction("index", "Home");

            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            return RedirectToAction("index", "Home");
        }

        public ActionResult Details()
        {
            var cookie = Request.Cookies["userId"];
            if (cookie == null) return RedirectToAction("index", "Home");
            var user = userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));

            var model = new AccountPageViewModel
                (
                    user.Id,
                    user.Fund,
                    trackRepository.GetPurchases(user).ToList()
                );

            return View(model);
        }

        public ActionResult AdminPage(int? page)
        {
            var cookie = Request.Cookies["userId"];
            if (cookie == null)
            {
                return RedirectToAction("LoginForm", "User");
            }
            if (string.IsNullOrEmpty(cookie.Values["userId"]))
            {
                return RedirectToAction("LoginForm", "User");
            }

            var user = userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));

            if (!user.IsAdmin)
            {
                return RedirectToAction("LoginForm", "User");
            }

            var tracks = trackRepository.GetAll();

            var pageNumber = page ?? 1;
            var onePageOfTracks = tracks.ToPagedList(pageNumber, 8);

            ViewBag.Items = onePageOfTracks;

            return View();
        }

        [HttpGet]
        public ActionResult GetUser(int id)
        {
            return Json(userRepository.GetById(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddFund(int id, int value)
        {
            var user = userRepository.GetById(id);
            user.Fund += value;

            userRepository.Update(user);

            return RedirectToAction("Details");

        }
    }
}