using System.Web.Helpers;
using System.Web.Mvc;
using BattleSea.Models;

namespace BattleSea.Controllers
{
    public class HomeController : Controller
    {
        private static readonly BattleField BattleField = new BattleField(8);

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        public JsonResult GetCurrentBattlefield()
        {
            return Json(BattleField.Cells);
        }

        public JsonResult GetDefaultShips()
        {
            return Json(BattleField.GetDefaultShips());
        }
    }
}
