using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BattleSea.Models;
using BattleSea.Models.Enums;
using Microsoft.AspNet.SignalR;
using WebGrease.Css.Extensions;

namespace BattleSea.Controllers
{
    public class GameController : GameContext
    {
        public GameController()
        {
            //subscribe to ship destroyed event
            Game.FirstPlayer.BattleField.ShipDestroyed += FirstPlayerBattleFieldOnShipDestroyed;
            Game.SecondPlayer.BattleField.ShipDestroyed += SecondPlayerBattleFieldOnShipDestroyed;
        }

        private readonly IHubContext _hubContext = GlobalHost.ConnectionManager.GetHubContext<BattleSeaHub>();

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
            srConnections.ForEach(c => _hubContext
                .Clients
                .Client(c.ToString())
                .opponentFire(new
                {
                    coordinate,
                    fireResult
                }));

            if (Game.Turn == Turn.FirstPlayer)
            {
                Game.FirstPlayer.GetSignalRConnections().ForEach(c => _hubContext.Clients.Client(c.ToString()).unlockField());
                Game.SecondPlayer.GetSignalRConnections().ForEach(c => _hubContext.Clients.Client(c.ToString()).lockField());
            }
            else if (Game.Turn == Turn.SecondPlayer)
            {
                Game.SecondPlayer.GetSignalRConnections().ForEach(c => _hubContext.Clients.Client(c.ToString()).unlockField());
                Game.FirstPlayer.GetSignalRConnections().ForEach(c => _hubContext.Clients.Client(c.ToString()).lockField());
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

        private void FirstPlayerBattleFieldOnShipDestroyed(object sender, ShipDestroyedEventArgs shipDestroyedEventArgs)
        {
            SurroundDestroyedShip(Game.SecondPlayer, (BattleField)sender, shipDestroyedEventArgs.SurroundedCells);
        }

        private void SecondPlayerBattleFieldOnShipDestroyed(object sender, ShipDestroyedEventArgs shipDestroyedEventArgs)
        {
            SurroundDestroyedShip(Game.FirstPlayer, (BattleField)sender, shipDestroyedEventArgs.SurroundedCells);
        }

        private void SurroundDestroyedShip(Player player, BattleField battleField, IEnumerable<Cell> surroundedCells)
        {
            player.GetSignalRConnections().ForEach(c => _hubContext.Clients.Client(c.ToString()).surroundShip(surroundedCells));
        }

        protected override void Dispose(bool disposing)
        {
            //unsubscribe to ship destroyed event
            Game.FirstPlayer.BattleField.ShipDestroyed -= FirstPlayerBattleFieldOnShipDestroyed;
            Game.SecondPlayer.BattleField.ShipDestroyed -= SecondPlayerBattleFieldOnShipDestroyed;

            base.Dispose(disposing);
        }
    }
}