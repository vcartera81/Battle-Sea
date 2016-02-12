using System;
using System.Linq;
using BattleSea.Models;
using BattleSea.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleSeaTest
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void CanInitializePlayer()
        {
            var player = new Player(10);
            Assert.IsTrue(player.Id == Guid.Empty);

            player.InitPlayer();
            Assert.IsTrue(player.Id != Guid.Empty);
        }

        [TestMethod]
        public void CanObfuscateBattleField()
        {
            var player = new Player(10);
            player.BattleField.PlaceShip(new Ship(
                ShipType.FourDeck,
                ShipOrientation.Horizontal,
                new Coordinate('a', 2)));

            Assert.IsTrue(player.BattleField.Field.AsSingleCollection().Any(c => c.State == CellState.ShipDeck));
            var obfuscatedBattlefield = player.GetObfuscatedBattlefield();

            Assert.IsTrue(obfuscatedBattlefield
                .BattleField
                .Field
                .AsSingleCollection()
                .All(c => c.State != CellState.ShipDeck));
        }

        [TestMethod]
        public void CanRegisterAndGetSignalRConnections()
        {
            var player = new Player(10);

            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var guid3 = Guid.NewGuid();

            var guids = new[] { guid1, guid2, guid3 };

            player.RegisterSignalRConnection(guids[0]);
            player.RegisterSignalRConnection(guids[1]);
            player.RegisterSignalRConnection(guids[2]);

            foreach (var guid in player.GetSignalRConnections())
            {
                Assert.IsTrue(player.GetSignalRConnections().Contains(guid));
            }
        }
    }
}
