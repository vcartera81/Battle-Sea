using System;
using System.Web.Mvc;
using BattleSea.Models;
using BattleSea.Models.Enums;
using Microsoft.AspNet.SignalR;
using WebGrease.Css.Extensions;

namespace BattleSea.Controllers
{
    public class GameController : GameContext
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
                Opponent = Game.GetPlayerById(PlayerId, true).ObfuscateBattlefield()
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
            var fireResult = Game.GetPlayerById(PlayerId, true).BattleField.Fire(coordinate);

            var srConnections = Game.GetPlayerById(PlayerId, true).GetSignalRConnections();
            var srContext = GlobalHost.ConnectionManager.GetHubContext<BattleSeaHub>();
            srConnections.ForEach(c => srContext
                .Clients
                .Client(c.ToString())
                .opponentFire(new
                {
                    coordinate,
                    fireResult
                }));

            if (Game.Turn == Turn.FirstPlayer)
            {
                Game.FirstPlayer.GetSignalRConnections().ForEach(c => srContext.Clients.Client(c.ToString()).unlockField());
                Game.SecondPlayer.GetSignalRConnections().ForEach(c => srContext.Clients.Client(c.ToString()).lockField());
            }
            else if (Game.Turn == Turn.SecondPlayer)
            {
                Game.SecondPlayer.GetSignalRConnections().ForEach(c => srContext.Clients.Client(c.ToString()).unlockField());
                Game.FirstPlayer.GetSignalRConnections().ForEach(c => srContext.Clients.Client(c.ToString()).lockField());
            }
            
            return Json(fireResult);
        }

        [HttpPost]
        public void Start()
        {
            Game.Start();
        }

        [HttpPost]
        public JsonResult RegisterPlayer(Guid connection)
        {
            SetPlayerSignalRConnectionId(connection);
            return new JsonResult();
        }
    }
}