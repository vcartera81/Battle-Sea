using System;
using System.Web.Mvc;

namespace BattleSea.Controllers
{
    public class GameController : BattleControllerBase
    {
        // GET: Game
        public ActionResult Index(Guid id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCurrentBattlefield()
        {
            return Json(Game);
        }

        [HttpPost]
        public void ShuffleShips()
        {
            if (!Game.Started)
                Game.FirstPlayer.BattleField.PlaceShipsRandomly();
        }
    }
}