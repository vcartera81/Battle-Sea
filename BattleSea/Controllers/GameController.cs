using System;
using System.Web.Mvc;
using BattleSea.Models;

namespace BattleSea.Controllers
{
    public class GameController : BattleControllerBase
    {
        public ActionResult Index(Guid id, Guid playerId)
        {
            if (id != Game.Id)
                throw new Exception("Game not found");

            return View(Game);
        }

        [HttpPost]
        public JsonResult GetCurrentBattlefield()
        {
            return Json(new GameViewModel
            {
                State = Game.State,
                PlayerId = PlayerId,
                You = Game.GetPlayerById(PlayerId),
                Opponent = Game.GetPlayerById(PlayerId, true)/*.ObfuscateBattlefield()*/
            });
        }

        [HttpPost]
        public void ShuffleShips()
        {
            if (Game.State == GameState.Initialized)
                Game.GetPlayerById(PlayerId).BattleField.PlaceShipsRandomly();
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