using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BattleSea.Models.Enums;

namespace BattleSea.Models
{
    public struct Cell
    {
        public CellState State { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}