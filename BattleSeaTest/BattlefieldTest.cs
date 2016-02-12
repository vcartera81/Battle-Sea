using System.Collections.Generic;
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
        public void CanGetProperSurroundedCoordinatesOfTheShip()
        {
            var field = new BattleField(10);
            var ship = GetFourDeckShip(ShipOrientation.Horizontal);

            //change ship's starting point
            ship.StartingPoint = new Coordinate('b', 1);

            //lets create a collection of expected surrounded cells
            var expectedCoordinates = new List<Coordinate>
            {
                new Coordinate('a', 0),
                new Coordinate('b', 0),
                new Coordinate('c', 0),
                new Coordinate('d', 0),
                new Coordinate('e', 0),
                new Coordinate('f', 0),
                new Coordinate('f', 1),
                new Coordinate('f', 2),
                new Coordinate('e', 2),
                new Coordinate('d', 2),
                new Coordinate('c', 2),
                new Coordinate('b', 2),
                new Coordinate('a', 2),
                new Coordinate('a', 1)
            };

            //persist ship on map
            field.PlaceShip(ship);
            var results = field.GetSurroundedCoordinatesOfTheShip(ship);
            Assert.AreEqual(expectedCoordinates.Count, results.Count());

            foreach (var coordinate in results)
            {
                Assert.IsTrue(expectedCoordinates.Contains(coordinate));
            }
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

        [TestMethod]
        public void WillNotBePossibleToPlaceAShipInCloseProximityToOther()
        {
            var field = new BattleField(10);

            var ship1 = GetFourDeckShip(ShipOrientation.Horizontal);
            var ship2 = GetFourDeckShip(ShipOrientation.Horizontal);
            ship2.StartingPoint = new Coordinate('a', 2);

            //persist first ship
            var resultPlaceShip1 = field.PlaceShip(ship1);
            var resultPlaceShip2 = field.PlaceShip(ship2);

            foreach (var coordinate in ship1.GetCoordinates())
            {
                Assert.IsTrue(field.Field[coordinate].State == CellState.ShipDeck);
            }

            Assert.IsTrue(resultPlaceShip1);
            Assert.IsFalse(resultPlaceShip2);

            foreach (var coordinate in ship2.GetCoordinates())
            {
                Assert.IsTrue(field.Field[coordinate].State == CellState.Empty);
            }
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
