using System.Web.Mvc;
using BattleSea.Models;

namespace BattleSea.Controllers
{
    public class HomeController : Controller
    {
        private readonly Game _game;

        public HomeController()
        {
            if (System.Web.HttpContext.Current.Session["Game"] == null)
                InitGame();

            _game = System.Web.HttpContext.Current.Session["Game"] as Game;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        public JsonResult GetCurrentBattlefield()
        {
            return Json(_game);
        }

        public void ShuffleShips()
        {
            if (!_game.Started)
                _game.FirstPlayer.BattleField.PlaceShipsRandomly();
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
