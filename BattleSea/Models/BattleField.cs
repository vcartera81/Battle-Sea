using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace BattleSea.Models
{
    public class BattleField
    {
        private readonly Random _random = new Random();
        private readonly int _size;

        public BattleFieldCells Field { get; private set; }

        public IEnumerable<Ship> Ships { get; set; }

        public BattleField(int size)
        {
            _size = size;
            Field = new BattleFieldCells(size);
        }

        public BattleFieldCells PlaceShipsRandomly()
        {
            //wipe cells
            Field = new BattleFieldCells(_size);

            foreach (var ship in _shipsDefaultCollection)
            {
                var cellsCollection = Field.AsSingleCollection().Where(c => !Field.ProximityCheck(c.Coordinate) && c.State != CellState.ShipDeck).ToList();
                if (!cellsCollection.Any()) break;

                Func<Coordinate> getRandomCoordinate = () => cellsCollection[_random.Next(cellsCollection.Count() - 1)].Coordinate;
                ship.StartingPoint = getRandomCoordinate();

                while (!PlaceShip(ship))
                {
                    ship.SetRandomOrientation(_random);
                    ship.StartingPoint = getRandomCoordinate();
                }
            }

            return Field;
        }

        private bool PlaceShip(Ship ship)
        {
            var decks = (int)ship.Type;
            var possibleCoordinates = new Coordinate[decks];

            for (var i = 0; i < decks; i++)
            {
                //try to validate
                if (Field.ValidateCoordinate(ship.StartingPoint))
                {
                    //if there's already a ship on selected coordinate - invalidate ship position
                    if (Field[ship.StartingPoint] == CellState.ShipDeck) return false;

                    //proximity check
                    if (Field.ProximityCheck(ship.StartingPoint)) return false;

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
            possibleCoordinates.ForEach(c => Field[c] = CellState.ShipDeck);
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