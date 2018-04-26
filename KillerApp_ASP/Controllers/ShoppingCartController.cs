using System;
using System.Linq;
using System.Web.Mvc;
using KillerAppClassLibrary.Classes;
using KillerAppClassLibrary.Context.Interface;
using KillerAppClassLibrary.Context.Sql;
using KillerAppClassLibrary.Logic.Repositories;
using KillerApp_ASP.ViewModels;
using Controller = System.Web.Mvc.Controller;

namespace KillerApp_ASP.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartRepository shoppingCartRepository = new ShoppingCartRepository(new ShoppingCartSqlContext());
        private readonly UserRepository userRepository = new UserRepository(new UserSqlContext());
        private readonly TrackRepository trackRepository = new TrackRepository(new TrackSqlContext());
        private readonly OrderRepository orderRepository = new OrderRepository(new OrderSqlContext());
        

        public ActionResult Checkout(UserViewModel model)
        {
            var user = userRepository.GetById(model.Id);

            var tracks = shoppingCartRepository.GetAllItems(user);

            var order = new Order(DateTime.Now, tracks, user);

            orderRepository.Create(order);
            shoppingCartRepository.RemoveAllItems(user);

            return RedirectToAction("OrderFinished");
        }

        public ActionResult OrderFinished()
        {
            return View();
        }

        public ActionResult CheckoutView()
        {
            var cookie = Request.Cookies["userId"];
            if (cookie == null)
            {
                return RedirectToAction("LoginForm","User");
            }
            if (string.IsNullOrEmpty(cookie.Values["userId"]))
            {
                return RedirectToAction("LoginForm","User");
            }

            var user = userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));
            var shoppingCart = shoppingCartRepository.GetAllItems(user);

            var userModel = new UserViewModel(user.Id, user.Username, user.Email, user.Password, user.Fund);

            return View(new ShoppingCartViewModel(userModel, shoppingCart.ToList()));
        }

        public ActionResult Add(int id)
        {
            var track = trackRepository.GetById(id);

            var cookie = Request.Cookies["userId"];
            if (cookie == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (string.IsNullOrEmpty(cookie.Values["userId"]))
            {
                return RedirectToAction("Login", "User");
            }

            var user = userRepository.GetById(Convert.ToInt32(cookie.Values["userId"]));

            if (shoppingCartRepository.GetAllItems(user).Contains(track))
            {
                return RedirectToAction("index", "Home");
            }

            shoppingCartRepository.Add(track, user);

            return RedirectToAction("CheckoutView");
        }

        public ActionResult Remove(int id)
        {
            var track = trackRepository.GetById(id);

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

            shoppingCartRepository.RemoveItems(user, track);

            return RedirectToAction("CheckoutView");
        }
    }
}