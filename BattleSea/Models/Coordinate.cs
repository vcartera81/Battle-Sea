namespace BattleSea.Models
{
    public struct Coordinate
    {
        private char _column;

        public Coordinate(char column, int row) : this()
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
    }
}