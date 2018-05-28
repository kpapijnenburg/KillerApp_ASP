using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KillerAppClassLibrary.Classes;
using KillerAppClassLibrary.Context.Sql;
using KillerAppClassLibrary.Logic.Repositories;
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
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }

            return View(new UserViewModel());
        }

        public ActionResult RegisterForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserViewModel model)
        {
            RedirectToRouteResult result;

            var user = new User
                (
                model.Username,
                model.Email,
                model.Password
                );

            try
            {
                userRepository.Create(user);
                result = RedirectToAction("index", "Home");
            }
            catch (Exception e)
            {
                ViewBag.Message = "Registration has failed" + e;
                result = RedirectToAction("RegisterForm", "User");
            }

            return result;
        }

        [HttpPost]
        public ActionResult Login(UserViewModel model)
        {
            RedirectToRouteResult result;

            try
            {
                var user = userRepository.Login(model.Email, model.Password);

                var cookie = new HttpCookie("userId");
                cookie.Values.Add("userId", user.Id.ToString());
                cookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(cookie);

                result = RedirectToAction("index", "Home");
            }
            catch (Exception e)
            {
                TempData["Message"] = "Email or password is invalid.";
                result = RedirectToAction("LoginForm", "User");
            }

            return result;

        }

        public ActionResult Logout()
        {
            var cookie = Request.Cookies["userId"];
            if (cookie == null) RedirectToAction("index", "Home");

            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        public ActionResult Details(int? page)
        {
            var cookie = Request.Cookies["userId"];
            if (cookie == null) RedirectToAction("index", "Home");
            var user = userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));

            var purchases = trackRepository.GetPurchases(user).ToList();

            int pageNumber = page ?? 1;
            var onePageOfPurchases = purchases.ToPagedList(pageNumber, 10);
            ViewBag.Items = onePageOfPurchases;

            var model = new AccountPageViewModel
                (
                    user.Id,
                    user.Fund
                );

            return View(model);
        }

        [HttpGet]
        public ActionResult AdminPage(int? page)
        {
            var cookie = Request.Cookies["userId"];
            if (cookie == null)
            {
                RedirectToAction("LoginForm", "User");
            }
            if (string.IsNullOrEmpty(cookie.Values["userId"]))
            {
                RedirectToAction("LoginForm", "User");
            }

            var user = userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));

            if (!user.IsAdmin)
            {
                RedirectToAction("LoginForm", "User");
            }

            var tracks = trackRepository.GetAll();

            int pageNumber = page ?? 1;
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