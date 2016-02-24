using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BattleSea.Models;
using BattleSea.Models.Enums;

namespace BattleSea.Controllers
{
    public abstract class GameContext : Controller
    {
        private const int DefaultBattlefieldSize = 10;
        protected Game Game { get; private set; }
        protected Player Player { get; private set; }

        private static readonly HashSet<Game> Games;
        private static readonly HashSet<Player> Players;

        protected int GamesCount => Games.Count;
        protected int PlayersCount => Players.Count;

        static GameContext()
        {
            Games = new HashSet<Game>();
            Players = new HashSet<Player>();
        }

        protected GameContext(bool shouldCreatePlayer = false)
        {
            IdentifyPlayer(shouldCreatePlayer);
            IdentifyGame();
        }

        private void IdentifyGame()
        {
            var matchedGame = Games.FirstOrDefault(g => g.HasPlayer(Player.Id));
            if (matchedGame != null) { Game = matchedGame; return; }

            //lookup for games without first player
            var game = Games.FirstOrDefault(g => !g.FirstPlayer.IsAvailable && g.State == GameState.Initialized);

            if (game != null)
            {
                Game = game;
                Game.FirstPlayer = Player;
                return;
            }

            //lookup games without second player
            game = Games.FirstOrDefault(g => !g.SecondPlayer.IsAvailable && g.State == GameState.Initialized);

            if (game != null)
            {
                Game = game;
                Game.SecondPlayer = Player;
                return;
            }

            //if no: create new game, attach as first player
            Game = CreateGame();
            Game.FirstPlayer = Player;
        }

        private void IdentifyPlayer(bool shouldCreatePlayer)
        {
            var urlReferer = System.Web.HttpContext.Current.Request.UrlReferrer;
            if (urlReferer == null && shouldCreatePlayer)
            {
                CreateAndAddPlayer();
                return;
            }

            var playerId = HttpUtility.ParseQueryString(System.Web.HttpContext.Current.Request.Url.Query)["playerId"] ??
                           HttpUtility.ParseQueryString(System.Web.HttpContext.Current.Request.UrlReferrer.Query)["playerId"];

            Player = Players.First(p => p.Id == Guid.Parse(playerId));
        }

        private void CreateAndAddPlayer()
        {
            var playerInstance = new Player(DefaultBattlefieldSize);
            playerInstance.Initialize();
            Players.Add(playerInstance);
            Player = playerInstance;
        }

        protected void SetPlayerSignalRConnectionId(Guid connection)
        {
            Game.GetPlayerById(Player.Id).RegisterSignalRConnection(connection);
        }

        private static Game CreateGame()
        {
            var game = new Game(DefaultBattlefieldSize);
            Games.Add(game);
            return game;
        }
    }
}