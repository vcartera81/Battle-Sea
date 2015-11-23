using System.Web.Mvc;

namespace BattleSea.Controllers
{
    public class HomeController : BattleControllerBase
    {
        public RedirectToRouteResult Index()
        {
            return RedirectToAction("Index", "Game", new { id = Game.Id });
        }
    }
}
