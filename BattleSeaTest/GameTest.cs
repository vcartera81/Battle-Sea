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
            try
            {
                game.Start();
            }
            catch (InvalidGameStateException)
            {
                game.FirstPlayer.InitPlayer();
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

        private static Game BuildGame()
        {
            var game = new Game(10);
            game.FirstPlayer.InitPlayer();
            game.SecondPlayer.InitPlayer();
            game.Start();

            return game;
        }
    }
}
