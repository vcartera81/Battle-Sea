using System;
using System.Collections.Generic;
using BattleSea.Models.Enums;

namespace BattleSea.Models
{
    public class BattleFieldCells
    {
        private readonly int _size;
        private IList<IList<Cell>> _matrix;

        public IEnumerable<IEnumerable<Cell>> Cells => _matrix;

        public Cell this[Coordinate coord]
        {
            get { return _matrix[coord.Row][coord.Column - 65]; }
            set { _matrix[coord.Row][coord.Column - 65] = value; }
        }

        public BattleFieldCells(int size)
        {
            _size = size;
            Initialize();
        }

        public bool ValidateCoordinate(Coordinate coordinate)
        {
            //try to validate coordinate
            try
            {
                var validator = this[coordinate];
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        public bool ProximityCheck(Coordinate requestedCoordinate)
        {
            var coordinate = Coordinate.Copy(requestedCoordinate);
            Func<Coordinate, bool> checker = (coord) =>
            {
                try
                {
                    var check = this[coord];
                    if (this[coord].State == CellState.ShipDeck)
                        return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }

                return false;
            };

            //move up
            if (checker(coordinate.MoveUp())) return true;
            if (checker(coordinate.MoveLeft())) return true;
            if (checker(coordinate.MoveDown())) return true;
            if (checker(coordinate.MoveDown())) return true;
            if (checker(coordinate.MoveRight())) return true;
            if (checker(coordinate.MoveRight())) return true;
            return checker(coordinate.MoveUp()) || checker(coordinate.MoveUp());
        }

        public IReadOnlyList<Cell> AsSingleCollection()
        {
            var collection = new List<Cell>(_size * _size);
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    var coordinate = new Coordinate((char) (j + 65), i);
                    collection.Add(new Cell
                    {
                        Coordinate = coordinate,
                        State = this[coordinate].State
                    });
                }
            }

            return collection;
        } 

        private void Initialize()
        {
            _matrix = new List<IList<Cell>>(_size);
            for (var i = 0; i < _size; i++)
            {
                _matrix.Add(new List<Cell>(_size));
                for (var j = 0; j < _size; j++)
                {
                    _matrix[i].Add(new Cell { State = CellState.Empty, Coordinate = new Coordinate((char)(j + 65), i) });
                }
            }
        }
    }
}