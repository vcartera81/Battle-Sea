using System;
using BattleSea.Models;
using BattleSea.Models.Enums;
using BattleSea.Models.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleSeaTest
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void WillThrowExceptionIfTryToStartAGameWithoutOneOrBothPlayers()
        {
            var game = new Game(10);

            var firstPlayer = new Player(10);
            var secondPlayer = new Player(10);

            game.FirstPlayer = firstPlayer;
            game.SecondPlayer = secondPlayer;

            try
            {
                game.Start();
            }
            catch (InvalidGameStateException)
            {
                game.FirstPlayer.Initialize();
                try
                {
                    game.Start();
                }
                catch (InvalidGameStateException)
                {
                    return;
                }
            }

            Assert.Fail();
        }

        [TestMethod]
        public void WillThrowExceptionIfTryToStartAnAlreadyStartedGame()
        {
            var game = BuildGame();

            try
            {
                game.Start();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {

            }
        }

        [TestMethod]
        public void WillChangeTurnIfPlayerDidNotShotADeck()
        {
            var game = BuildGame();
            var initialTurn = game.Turn;
            game.SecondPlayer.BattleField.Fire(new Coordinate('A', 1));
            Assert.AreNotEqual(initialTurn, game.Turn);

            initialTurn = game.Turn;
            game.FirstPlayer.BattleField.Fire(new Coordinate('C', 4));
            Assert.AreNotEqual(initialTurn, game.Turn);
        }

        [TestMethod]
        public void WillLetFireOnceAgainIfDeckWasShot()
        {
            var game = BuildGame();
            game.FirstPlayer.BattleField.PlaceShip(new Ship(
                ShipType.TwoDeck,
                ShipOrientation.Vertical,
                new Coordinate('A', 1)));

            game.SecondPlayer.BattleField.Fire(new Coordinate('A', 1));
            Assert.AreEqual(Turn.SecondPlayer, game.Turn);
        }

        [TestMethod]
        public void WillReturnTrueIfTheresAPlayerWithRequestedIdAndFalseIfNot()
        {
            var game = new Game(10);
            var player = new Player(10);
            player.Initialize();
            game.FirstPlayer = player;

            Assert.IsTrue(game.HasPlayer(player.Id));
            Assert.IsFalse(game.HasPlayer(Guid.NewGuid()));

            var player2 = new Player(10);
            player2.Initialize();
            game.SecondPlayer = player2;

            Assert.IsTrue(game.HasPlayer(player2.Id));
            game.FirstPlayer = null;

            Assert.IsTrue(game.HasPlayer(player2.Id));
        }

        private static Game BuildGame()
        {
            var game = new Game(10);

            var firstPlayer = new Player(10);
            firstPlayer.Initialize();

            var secondPlayer = new Player(10);
            secondPlayer.Initialize();

            game.FirstPlayer = firstPlayer;
            game.SecondPlayer = secondPlayer;

            game.Start();

            return game;
        }
    }
}
