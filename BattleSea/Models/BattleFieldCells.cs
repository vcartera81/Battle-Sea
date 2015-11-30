using System;
using System.Collections.Generic;
using BattleSea.Models.Enums;

namespace BattleSea.Models
{
    public class BattleFieldCells
    {
        private readonly int _size;
        private IList<IList<CellState>> _matrix;

        public IEnumerable<IEnumerable<CellState>> Cells => _matrix;

        public CellState this[Coordinate coord]
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
                    if (this[coord] == CellState.ShipDeck)
                        return true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }

                return false;
            };

            //move up
            if (checker(coordinate.IncreaseRow())) return true;
            if (checker(coordinate.IncreaseColumn())) return true;
            if (checker(coordinate.DecreaseRow())) return true;
            if (checker(coordinate.DecreaseRow())) return true;
            if (checker(coordinate.DecreaseColumn())) return true;
            if (checker(coordinate.DecreaseColumn())) return true;
            return checker(coordinate.IncreaseRow()) || checker(coordinate.IncreaseRow());
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
                        State = this[coordinate]
                    });
                }
            }

            return collection;
        } 

        private void Initialize()
        {
            _matrix = new List<IList<CellState>>(_size);
            for (var i = 0; i < _size; i++)
            {
                _matrix.Add(new List<CellState>(_size));
                for (var j = 0; j < _size; j++)
                {
                    _matrix[i].Add(CellState.Empty);
                }
            }
        }
    }
}