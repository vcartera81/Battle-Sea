using System;
using WebGrease.Css.Extensions;

namespace BattleSea.Models
{
    public class Player
    {
        private readonly int _fieldSize;

        public Player(int fieldSize)
        {
            BattleField = new BattleField(fieldSize);
            _fieldSize = fieldSize;
        }

        public Guid Id { get; private set; }

        public bool IsAvailable => Id != Guid.Empty;

        public BattleField BattleField { get; set; }

        public Guid InitPlayer()
        {
            Id = Guid.NewGuid();
            return Id;
        }

        public Player ObfuscateBattlefield()
        {
            //hide battlefield ships
            var allCells = BattleField.Field.AsSingleCollection();
            var censoredBattleField = new BattleField(_fieldSize);
            allCells.ForEach(c => censoredBattleField.Field[c.Coordinate] = c.State == CellState.ShipDeck ? CellState.Empty : c.State);
            return new Player(_fieldSize)
            {
                Id = Id,
                BattleField = censoredBattleField
            };
        }
    }
}