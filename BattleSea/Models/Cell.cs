using System;
using BattleSea.Models.Enums;

namespace BattleSea.Models
{
    public struct Cell
    {
        public Guid? ShipId { get; set; }
        public CellState State { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}