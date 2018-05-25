using System;
using System.Web.Mvc;
using KillerAppClassLibrary.Classes;
using KillerAppClassLibrary.Context.Sql;
using KillerAppClassLibrary.Logic.Repositories;
using Controller = System.Web.Mvc.Controller;

namespace KillerApp_ASP.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserRepository userRepository = new UserRepository(new UserSqlContext());
        private readonly CommentRepository commentRepository = new CommentRepository(new CommentSqlContext());

        [HttpPost]
        public ActionResult Create(int trackid, int userid, string content)
        {
            var comment = new Comment
            (
                trackid,
                userRepository.GetById(userid),
                DateTime.Now,
                content
            );

            commentRepository.Create(comment);

            return RedirectToAction("Details", "Track", new {id = trackid});

        }

        public ActionResult Delete(int id, int trackId)
        {
            commentRepository.Delete(id);

            return RedirectToAction("Details", "Track", new {id = trackId});
        }
    }
}