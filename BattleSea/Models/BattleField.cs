using System;
using System.Collections.Generic;
using System.Linq;
using BattleSea.Models.Enums;
using BattleSea.Models.Exceptions;
using WebGrease.Css.Extensions;

namespace BattleSea.Models
{
    public class BattleField
    {
        #region FIELDS

        private readonly Random _random = new Random(DateTime.Now.Millisecond);
        private readonly int _size;
        private readonly IList<Ship> _ships;

        #endregion

        #region EVENT HANDLERS

        public delegate void FiredEventHandler(object sender, FiredEventArgs e);
        public event FiredEventHandler Fired;
        public delegate void ShipDestroyedEventHandler(object sender, ShipDestroyedEventArgs e);
        public event ShipDestroyedEventHandler ShipDestroyed;

        #endregion

        #region PUBLIC

        public BattleFieldCells Field { get; private set; }

        public IEnumerable<Ship> Ships => _ships;

        public bool AllShipsDestroyed
            => !_ships.Any(s => s.ShipState == ShipState.Clean || s.ShipState == ShipState.Wounded);

        public BattleField(int size)
        {
            _size = size;
            Field = new BattleFieldCells(size);
            _ships = new List<Ship>(_shipsDefaultCollection.Count());
        }

        public void PlaceShipsRandomly()
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
        }

        public Cell Fire(Coordinate coordinate)
        {
            switch (Field[coordinate].State)
            {
                case CellState.ShipDeck:
                    Field[coordinate] = new Cell { State = CellState.Exploded, Coordinate = coordinate, ShipId = Field[coordinate].ShipId };
                    var targetShip = _ships.First(s => s.Id == Field[coordinate].ShipId.Value);
                    targetShip.Shot();

                    //invoke ship destroyed event
                    if (targetShip.ShipState == ShipState.Destroyed)
                    {
                        var surroundedCells = new List<Cell>();
                        //mark nearby cells as surrounded
                        GetSurroundedCoordinates(targetShip).ForEach(c =>
                        {
                            var cell = Field[c];
                            if (cell.State == CellState.Shot) cell.State = CellState.ShotAndSurrounded;
                            else if (cell.State == CellState.Empty) cell.State = CellState.Surrounded;
                            Field[c] = cell;
                            surroundedCells.Add(cell);
                        });

                        //invoke
                        ShipDestroyed?.Invoke(this, new ShipDestroyedEventArgs { Ship = targetShip, SurroundedCells = surroundedCells });
                    }
                    break;
                case CellState.Empty:
                    Field[coordinate] = new Cell { State = CellState.Shot, Coordinate = coordinate };
                    break;
                case CellState.Exploded:
                    throw new InvalidShotException("Cannot shot again an exploded cell.");
                case CellState.Shot:
                    throw new InvalidShotException("This cell were already shot.");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //call event
            Fired?.Invoke(this, new FiredEventArgs { Coordinate = coordinate, Result = Field[coordinate].State });

            return Field[coordinate];
        }

        public IEnumerable<Coordinate> GetSurroundedCoordinates(Ship ship)
        {
            var result = new List<Coordinate>();
            var shipCoordinates = ship.GetCoordinates();
            foreach (var shipCoord in shipCoordinates)
            {
                var mutableCoordinate = Coordinate.Copy(shipCoord);
                if (Field.ValidateCoordinate(mutableCoordinate.MoveDown())) result.Add(Coordinate.Copy(mutableCoordinate));
                if (Field.ValidateCoordinate(mutableCoordinate.MoveLeft())) result.Add(Coordinate.Copy(mutableCoordinate));
                if (Field.ValidateCoordinate(mutableCoordinate.MoveUp())) result.Add(Coordinate.Copy(mutableCoordinate));
                if (Field.ValidateCoordinate(mutableCoordinate.MoveUp())) result.Add(Coordinate.Copy(mutableCoordinate));
                if (Field.ValidateCoordinate(mutableCoordinate.MoveRight())) result.Add(Coordinate.Copy(mutableCoordinate));
                if (Field.ValidateCoordinate(mutableCoordinate.MoveRight())) result.Add(Coordinate.Copy(mutableCoordinate));
                if (Field.ValidateCoordinate(mutableCoordinate.MoveDown())) result.Add(Coordinate.Copy(mutableCoordinate));
                if (Field.ValidateCoordinate(mutableCoordinate.MoveDown())) result.Add(Coordinate.Copy(mutableCoordinate));
            }
            return result.Where(c => !shipCoordinates.Contains(c)).Distinct().ToList();
        }

        #endregion

        #region PRIVATE

        private bool PlaceShip(Ship ship)
        {
            var decks = (int)ship.ShipType;
            var possibleCoordinates = new Coordinate[decks];
            var startingPoint = Coordinate.Copy(ship.StartingPoint);

            for (var i = 0; i < decks; i++)
            {
                //try to validate
                if (Field.ValidateCoordinate(startingPoint))
                {
                    //proximity check
                    if (Field.ProximityCheck(startingPoint)) return false;
                    possibleCoordinates[i] = Coordinate.Copy(startingPoint);

                    if (ship.ShipOrientation == ShipOrientation.Vertical)
                        startingPoint.MoveDown();
                    else
                        startingPoint.MoveRight();
                }
                else
                    return false;
            }

            //if everything went fine, persist possibleCoordinates on map
            possibleCoordinates.ForEach(c => Field[c] = new Cell { State = CellState.ShipDeck, Coordinate = c, ShipId = ship.Id });

            //add ship to collection
            _ships.Add(ship);

            return true;
        }

        private readonly IEnumerable<Ship> _shipsDefaultCollection = new List<Ship>
        {
            new Ship(ShipType.FourDeck),
            new Ship(ShipType.FourDeck),
            new Ship(ShipType.ThreeDeck),
            new Ship(ShipType.ThreeDeck),
            new Ship(ShipType.ThreeDeck),
            new Ship(ShipType.TwoDeck),
            new Ship(ShipType.TwoDeck),
            new Ship(ShipType.TwoDeck),
            new Ship(ShipType.OneDeck),
            new Ship(ShipType.OneDeck),
            new Ship(ShipType.OneDeck),
            new Ship(ShipType.OneDeck)
        };

        #endregion
    }

    public class FiredEventArgs : EventArgs
    {
        public Coordinate Coordinate { get; set; }
        public CellState Result { get; set; }
    }

    public class ShipDestroyedEventArgs : EventArgs
    {
        public Ship Ship { get; set; }
        public IEnumerable<Cell> SurroundedCells { get; set; }
    }
}