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
        private readonly UserRepository userRepository = new UserRepository(new UserContextSqlContext());
        private readonly CommentRepository commentRepository = new CommentRepository(new CommentSqlContext());
        private readonly TrackRepository trackRepository = new TrackRepository(new TrackSqlContext());

        [HttpPost]
        public ActionResult Create(int trackid, int userid, string content)
        {
            var comment = new Comment
            (
                userRepository.GetById(userid),
                DateTime.Now,
                content
            );

            var track = trackRepository.GetById(trackid);
            
            commentRepository.Create(comment, track);

            return RedirectToAction("Details", "Track", new {id = trackid});

        }

        public ActionResult Delete(int id, int trackId)
        {
            commentRepository.Delete(id);

            return RedirectToAction("Details", "Track", new {id = trackId});
        }
    }
}