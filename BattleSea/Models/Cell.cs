﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BattleSea.Models
{
    public struct Cell
    {
        public CellState State { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}