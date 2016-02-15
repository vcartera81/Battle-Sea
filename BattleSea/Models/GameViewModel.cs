using System;
using BattleSea.Models.Enums;

namespace BattleSea.Models
{
    public class GameViewModel
    {
        public Guid PlayerId { get; set; }

        public Player You { get; set; }

        public Player Opponent { get; set; }

        public GameState State { get; set; }

        public bool YourTurn { get; set; }
    }
}