using System.Web.Mvc;

namespace BattleSea.Controllers
{
    public class HomeController : GameContext
    {
        public RedirectToRouteResult Index()
        {
            return RedirectToAction(null, "Game", new { id = Game.Id, PlayerId });
        }
    }
}
