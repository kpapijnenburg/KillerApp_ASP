using System.Web.Mvc;
using KillerAppClassLibrary.Context.Sql;
using KillerAppClassLibrary.Logic.Repositories;
using Controller = System.Web.Mvc.Controller;

namespace KillerApp_ASP.Controllers
{
    public class VoteController : Controller
    {
        private readonly VoteRepository voteRepository;
        private readonly TrackRepository trackRepository;
        private readonly UserRepository userRepository;

        public VoteController()
        {
            voteRepository = new VoteRepository(new VoteSqlContext());
            trackRepository = new TrackRepository(new TrackSqlContext());
            userRepository = new UserRepository(new UserSqlContext());
        }

        
        public ActionResult Cast(int userid,int trackId, int value)
        {
            var user = userRepository.GetById(userid);
            var track = trackRepository.GetById(trackId);

            voteRepository.CastVote(user, track, value);

            return RedirectToAction("Details", "Track", new {id = trackId});

        }

        public ActionResult Delete(int trackid, int userid)
        {
            var user = userRepository.GetById(userid);
            var track = trackRepository.GetById(trackid);

            voteRepository.Delete(track, user);

            return RedirectToAction("Details","Track", new {id = trackid});
        }
    }
}