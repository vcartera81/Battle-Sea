using System;

namespace BattleSea.Models
{
    public class Coordinate
    {
        private char _column;

        public Coordinate(char column, int row)
        {
            Column = column;
            Row = row;
        }

        public char Column
        {
            get { return char.ToUpper(_column); }
            set { _column = value; }
        }

        public int Row { get; set; }

        public Coordinate DecreaseColumn()
        {
            _column++;
            return this;
        }

        public Coordinate IncreaseColumn()
        {
            _column--;
            return this;
        }

        public Coordinate IncreaseRow()
        {
            Row--;
            return this;
        }

        public Coordinate DecreaseRow()
        {
            Row++;
            return this;
        }

        public static Coordinate GetRandom(Random random, char maxColumn, int maxRow)
        {
            var randomColumn = random.Next(65, maxColumn + 1);
            var randomRow = random.Next(maxRow + 1);

            return new Coordinate((char)randomColumn, randomRow);
        }

        public static Coordinate Copy(Coordinate source)
        {
            return new Coordinate(source.Column, source.Row);
        }
    }
}