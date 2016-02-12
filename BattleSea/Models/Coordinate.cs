using System;

namespace BattleSea.Models
{
    public class Coordinate : IEquatable<Coordinate>
    {
        private char _column;

        public Coordinate(char column, int row)
        {
            Column = column;
            Row = row;
        }

        public Coordinate ()
        {
        }

        public char Column
        {
            get { return char.ToUpper(_column); }
            set { _column = value; }
        }

        public int Row { get; set; }

        public Coordinate MoveRight()
        {
            _column++;
            return this;
        }

        public Coordinate MoveLeft()
        {
            _column--;
            return this;
        }

        public Coordinate MoveUp()
        {
            Row--;
            return this;
        }

        public Coordinate MoveDown()
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

        public bool Equals(Coordinate other)
        {
            if (other == null) return false;
            return Column == other.Column && Row == other.Row;
        }

        public override int GetHashCode()
        {
            var row = Row;
            return (row ^ (int)(Column >> 32));
        }

        public override string ToString()
        {
            return $"{Column}-{Row}";
        }
    }
}