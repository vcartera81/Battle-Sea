using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BattleSea.Models;

namespace BattleSea.Controllers
{
    public abstract class GameContext : Controller
    {
        protected readonly Game Game;
        protected readonly Guid PlayerId;

        private static readonly ICollection<Game> Games;

        static GameContext()
        {
            Games = new List<Game>();
        }

        protected GameContext()
        {
            if (System.Web.HttpContext.Current.Session["PlayerId"] == null)
            {
                if (Games.Any(g => !g.FirstPlayer.IsAvailable && !g.SecondPlayer.IsAvailable) || !Games.Any())
                    InitGame();

                var gameWithoutFirstPlayer =
                    Games.FirstOrDefault(g => !g.FirstPlayer.IsAvailable);

                if (gameWithoutFirstPlayer != null)
                {
                    gameWithoutFirstPlayer.FirstPlayer.InitPlayer();
                    //gameWithoutFirstPlayer.FirstPlayer.BattleField.PlaceShipsRandomly();
                    PlayerId = gameWithoutFirstPlayer.FirstPlayer.Id;
                    System.Web.HttpContext.Current.Session["PlayerId"] = gameWithoutFirstPlayer.FirstPlayer.Id;
                    Game = gameWithoutFirstPlayer;
                    return;
                }

                var gameWithoutSecondPlayer =
                    Games.FirstOrDefault(g => !g.SecondPlayer.IsAvailable && g.FirstPlayer.IsAvailable);

                if (gameWithoutSecondPlayer != null)
                {
                    gameWithoutSecondPlayer.SecondPlayer.InitPlayer();
                    //gameWithoutSecondPlayer.SecondPlayer.BattleField.PlaceShipsRandomly();
                    PlayerId = gameWithoutSecondPlayer.SecondPlayer.Id;
                    System.Web.HttpContext.Current.Session["PlayerId"] = gameWithoutSecondPlayer.SecondPlayer.Id;
                    Game = gameWithoutSecondPlayer;
                }
            }
            else
            {
                PlayerId = (Guid)System.Web.HttpContext.Current.Session["PlayerId"];
                Game = Games.First(g => g.FirstPlayer.Id == PlayerId || g.SecondPlayer.Id == PlayerId);
            }
        }

        protected void SetPlayerSignalRConnectionId(Guid connection)
        {
            Game.GetPlayerById(PlayerId).RegisterSignalRConnection(connection);
        }

        private static void InitGame()
        {
            var game = new Game(10);
            Games.Add(game);
        }
    }
}