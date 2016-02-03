using System;
using System.Collections.Generic;
using BattleSea.Models.Enums;
using WebGrease.Css.Extensions;

namespace BattleSea.Models
{
    public class Player
    {
        private readonly int _fieldSize;
        private readonly HashSet<Guid> _signalRConnections = new HashSet<Guid>();

        public Player(int fieldSize) : this(fieldSize, new BattleField(fieldSize))
        {

        }

        private Player(int fieldSize, BattleField battleField, Guid id = default(Guid))
        {
            BattleField = battleField;
            _fieldSize = fieldSize;
            Id = id;
        }

        public Guid Id { get; private set; }

        public bool IsAvailable => Id != Guid.Empty;

        public BattleField BattleField { get; }

        public Player ObfuscateBattlefield()
        {
            //hide battlefield ships
            var allCells = BattleField.Field.AsSingleCollection();
            var censoredBattleField = new BattleField(_fieldSize);
            allCells.ForEach(c => censoredBattleField.Field[c.Coordinate] = c.State == CellState.ShipDeck ? new Cell { Coordinate = c.Coordinate, State = CellState.Empty } : c);

            return new Player(_fieldSize, censoredBattleField, Id);
        }

        public void RegisterSignalRConnection(Guid id)
        {
            if (id != Guid.Empty)
                _signalRConnections.Add(id);
        }

        public IEnumerable<Guid> GetSignalRConnections()
        {
            return _signalRConnections;
        }

        public void InitPlayer()
        {
            Id = Guid.NewGuid();
        }
    }
}