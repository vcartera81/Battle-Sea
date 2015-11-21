using System.Collections.Generic;

namespace BattleSea.Models
{
    public class BattleFieldCells
    {
        private readonly int _size;
        private IList<IList<CellState>> _matrix;

        public IEnumerable<IEnumerable<CellState>> Field => _matrix;

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

        private void Initialize()
        {
            _matrix = new List<IList<CellState>>(_size);
            for (var i = 0; i < _size; i++)
            {
                _matrix.Add(new List<CellState>(_size));
                for (var j = 0; j < _size; j++)
                {
                    _matrix[i].Add(CellState.Initialized);
                }
            }
        }
    }
}