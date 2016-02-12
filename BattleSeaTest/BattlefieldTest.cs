using System.Linq;
using BattleSea.Models;
using BattleSea.Models.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleSeaTest
{
    [TestClass]
    public class BattlefieldTest
    {
        [TestMethod]
        public void CanPersistShipOnTheField()
        {
            var battleField = new BattleField(10);
            battleField.PlaceShip(GetFourDeckShip());

            Assert.AreEqual(CellState.ShipDeck, battleField.Field[new Coordinate('a', 1)].State);
            Assert.AreEqual(CellState.ShipDeck, battleField.Field[new Coordinate('a', 2)].State);
            Assert.AreEqual(CellState.ShipDeck, battleField.Field[new Coordinate('a', 3)].State);
            Assert.AreEqual(CellState.ShipDeck, battleField.Field[new Coordinate('a', 4)].State);
            Assert.AreNotEqual(CellState.ShipDeck, battleField.Field[new Coordinate('a', 5)].State);
        }

        [TestMethod]
        public void CanCreateBattleField()
        {
            var battleField = new BattleField(10);
            Assert.AreEqual(battleField.Field.AsSingleCollection().Count, 10 * 10);
        }

        [TestMethod]
        public void CanPlaceRandomShips()
        {
            var field = new BattleField(10);
            field.PlaceShipsRandomly();
            Assert.AreEqual(field.ShipsDefaultCollection.Count(), field.Ships.Count());
        }

        [TestMethod]
        public void WillInvokeFiredEventWithTheRightArgs()
        {
            var eventInvoked = false;
            var field = new BattleField(10);
            field.PlaceShip(GetFourDeckShip());

            FiredEventArgs arguments = null;

            //subscribe
            field.Fired += (sender, args) =>
            {
                eventInvoked = true;
                arguments = args;
            };

            var fireCoordinate = new Coordinate('a', 2);
            field.Fire(fireCoordinate);
            Assert.IsTrue(eventInvoked);
            Assert.AreEqual(fireCoordinate, arguments.Coordinate);
            Assert.IsTrue(arguments.Result == CellState.Exploded);
        }

        [TestMethod]
        public void WillInvokeShipDestroyedEventWithTheRightArguments()
        {
            var eventInvoked = false;
            var ship = GetFourDeckShip();
            var field = new BattleField(10);
            field.PlaceShip(ship);

            ShipDestroyedEventArgs arguments = null;

            //subscribe
            field.ShipDestroyed += (sender, args) =>
            {
                eventInvoked = true;
                arguments = args;
            };

            field.Fire(new Coordinate('a', 1));
            field.Fire(new Coordinate('a', 2));
            field.Fire(new Coordinate('a', 3));
            field.Fire(new Coordinate('a', 4));

            Assert.IsTrue(eventInvoked);
            Assert.AreEqual(ship.Id, arguments.Ship.Id);
            Assert.IsTrue(arguments.SurroundedCells.Any());
        }

        private static Ship GetFourDeckShip(ShipOrientation orientation = ShipOrientation.Vertical)
        {
            return new Ship(
                ShipType.FourDeck,
                orientation,
                new Coordinate('a', 1));
        }
    }
}
