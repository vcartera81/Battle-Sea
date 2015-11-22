using System.Web.Helpers;
using System.Web.Mvc;
using BattleSea.Models;

namespace BattleSea.Controllers
{
    public class HomeController : Controller
    {
        private readonly BattleField _battleField = new BattleField(10);

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        public JsonResult GetCurrentBattlefield()
        {
            return Json(_battleField.PlaceShipsRandomly());
        }
    }
}
