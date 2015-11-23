using System.Web.Mvc;
using BattleSea.Models;

namespace BattleSea.Controllers
{
    public abstract class BattleControllerBase : Controller
    {
        protected readonly Game Game;

        protected BattleControllerBase()
        {
            if (System.Web.HttpContext.Current.Session["Game"] == null)
                InitGame();

            Game = System.Web.HttpContext.Current.Session["Game"] as Game;
        }

        private static void InitGame()
        {
            var game = new Game(10);
            game.FirstPlayer.BattleField.PlaceShipsRandomly();
            game.SecondPlayer.BattleField.PlaceShipsRandomly();
            System.Web.HttpContext.Current.Session["Game"] = game;
        }
    }
}