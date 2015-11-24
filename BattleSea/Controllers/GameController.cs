using System;
using System.Web.Mvc;
using BattleSea.Models;

namespace BattleSea.Controllers
{
    public class GameController : BattleControllerBase
    {
        public ActionResult Index(Guid id)
        {
            if (id != Game.Id)
                throw new Exception("Game not found");

            return View(Game);
        }

        [HttpPost]
        public JsonResult GetCurrentBattlefield()
        {
            return Json(Game);
        }

        [HttpPost]
        public void ShuffleShips()
        {
            if (Game.State == GameState.Initialized)
                Game.FirstPlayer.BattleField.PlaceShipsRandomly();
        }

        [HttpPost]
        public JsonResult Fire(Coordinate coordinate)
        {
            return Json(Game.SecondPlayer.BattleField.Fire(coordinate));
        }

        [HttpPost]
        public void Start()
        {
            Game.Start();
        }
    }
}