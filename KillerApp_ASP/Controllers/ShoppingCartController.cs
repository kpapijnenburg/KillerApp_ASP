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
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartRepository shoppingCartRepository = new ShoppingCartRepository(new ShoppingCartSqlContext());
        private readonly UserRepository userRepository = new UserRepository(new UserSqlContext());
        private readonly TrackRepository trackRepository = new TrackRepository(new TrackSqlContext());
        private readonly OrderRepository orderRepository = new OrderRepository(new OrderSqlContext());
        
        [HttpGet]
        public ActionResult Checkout(UserViewModel model)
        {
            var user = userRepository.GetById(model.Id);

            var tracks = shoppingCartRepository.GetAllItems(user);

            var order = new Order(DateTime.Now, tracks, user);

            orderRepository.Create(order);
            shoppingCartRepository.RemoveAllItems(user);

            return RedirectToAction("OrderFinished", model);
        }

        public ActionResult OrderFinished(UserViewModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult CheckoutView(int? page)
        {
            var user = GetUser();
            var shoppingCart = shoppingCartRepository.GetAllItems(user).ToList();
            decimal totalPrice = 0;

            foreach (var track in shoppingCart)
            {
                totalPrice += track.Price;
            }

            int pageNumber = page ?? 1;
            var onePageOfItems = shoppingCart.ToPagedList(pageNumber, 4);

            ViewBag.Items = onePageOfItems;

            return View(new ShoppingCartViewModel(user, totalPrice));
        }


        public ActionResult Add(int id)
        {
            RedirectToRouteResult result;

            var track = trackRepository.GetById(id);

            var user = GetUser();

            var shoppingCart = shoppingCartRepository.GetAllItems(user).ToList();

            bool matches = shoppingCart.Any(t => t.Id == track.Id);

            if (matches)
            {
                result =  RedirectToAction("Details", "Track", new {id});
            }
            else
            {
                shoppingCartRepository.Add(track, user);
                result = RedirectToAction("CheckoutView");
            }

            return result;
        }

        public ActionResult Remove(int id)
        {
            var track = trackRepository.GetById(id);

            var user = GetUser();

            shoppingCartRepository.RemoveItems(user, track);

            return RedirectToAction("CheckoutView");
        }


        public ActionResult AddLatestReleases()
        {
            var tracks = trackRepository.GetLatestReleases();

            var user = GetUser();

            var shoppingCart = shoppingCartRepository.GetAllItems(user).ToList();

            foreach (var track in tracks)
            {
                bool matches = shoppingCart.Any(t => t.Id == track.Id);

                if (!matches)
                {
                    shoppingCartRepository.Add(track, user);
                }
            }

            return RedirectToAction("CheckoutView");
        }


        public ActionResult AddMostDownloaded()
        {
            var tracks = trackRepository.GetMostDownloadedTracks();

            var user = GetUser();

            var shoppingCart = shoppingCartRepository.GetAllItems(user).ToList();

            foreach (var track in tracks)
            {
                bool matches = shoppingCart.Any(t => t.Id == track.Id);

                if (!matches)
                {
                    shoppingCartRepository.Add(track, user);
                }

                shoppingCartRepository.Add(track, user);
            }

            return RedirectToAction("CheckoutView");
        }

        [HttpGet]
        public User GetUser()
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

            return userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));
        }
    }
}