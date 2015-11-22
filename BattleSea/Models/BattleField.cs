using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace BattleSea.Models
{
    public class BattleField
    {
        private readonly BattleFieldCells _field;
        private readonly Random _random = new Random();

        public IEnumerable<Ship> Ships { get; set; }

        public BattleField(int size)
        {
            _field = new BattleFieldCells(size);
        }

        public BattleFieldCells PlaceShipsRandomly()
        {
            foreach (var ship in _shipsDefaultCollection)
            {
                var cellsCollection = _field.AsCollection().Where(c => !_field.ProximityCheck(c.Coordinate) && c.State != CellState.ShipDeck).ToList();
                if (!cellsCollection.Any()) break;

                Func<Coordinate> getRandomCoordinate = () => cellsCollection[_random.Next(cellsCollection.Count() - 1)].Coordinate;
                ship.StartingPoint = getRandomCoordinate();

                while (!PlaceShip(ship))
                {
                    ship.SetRandomOrientation(_random);
                    ship.StartingPoint = getRandomCoordinate();
                }
            }

            return _field;
        }

        private bool PlaceShip(Ship ship)
        {
            var decks = (int)ship.Type;
            var possibleCoordinates = new Coordinate[decks];

            for (var i = 0; i < decks; i++)
            {

                //try to validate
                if (_field.ValidateCoordinate(ship.StartingPoint))
                {
                    //if there's already a ship on selected coordinate - invalidate ship position
                    if (_field[ship.StartingPoint] == CellState.ShipDeck) return false;

                    //proximity check
                    if (_field.ProximityCheck(ship.StartingPoint)) return false;

                    possibleCoordinates[i] = Coordinate.Copy(ship.StartingPoint);

                    if (ship.Orientation == Orientation.Vertical)
                        ship.StartingPoint.DecreaseRow();
                    else
                        ship.StartingPoint.DecreaseColumn();
                }
                else
                    return false;
            }

            //if everything went fine, persist possibleCoordinates on map
            possibleCoordinates.ForEach(c => _field[c] = CellState.ShipDeck);
            return true;
        }

        private readonly IEnumerable<Ship> _shipsDefaultCollection = new List<Ship>
        {
            new Ship(Type.FourDeck),
            new Ship(Type.FourDeck),
            new Ship(Type.ThreeDeck),
            new Ship(Type.ThreeDeck),
            new Ship(Type.ThreeDeck),
            new Ship(Type.TwoDeck),
            new Ship(Type.TwoDeck),
            new Ship(Type.TwoDeck),
            new Ship(Type.OneDeck),
            new Ship(Type.OneDeck),
            new Ship(Type.OneDeck),
            new Ship(Type.OneDeck)
        };
    }
}