using System;
using WebGrease.Css.Extensions;

namespace BattleSea.Models
{
    public class Game
    {
        public Guid Id { get; }

        private readonly int _fieldSize;

        public Game(int fieldSize)
        {
            Id = Guid.NewGuid();
            FirstPlayer = new Player(fieldSize);
            SecondPlayer = new Player(fieldSize);
            _fieldSize = fieldSize;
        }

        public Player FirstPlayer { get; set; }
        internal Player SecondPlayer { get; set; }

        public Player SecondPlayerHiddenShips
        {
            get
            {
                //hide battlefield ships for second user
                var allCells = SecondPlayer.BattleField.Field.AsCollection();
                var censoredBattleField = new BattleField(_fieldSize);
                allCells.ForEach(c => censoredBattleField.Field[c.Coordinate] = c.State == CellState.ShipDeck ? CellState.Empty : c.State);
                return new Player(_fieldSize)
                {
                    BattleField = censoredBattleField
                };
            }
        }

        public bool Started { get; set; }
    }
}