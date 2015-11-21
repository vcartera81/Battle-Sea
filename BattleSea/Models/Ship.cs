namespace BattleSea.Models
{
    public class Ship
    {
        public enum ShipOrientation
        {
            Vertical,
            Horizontal
        }

        public enum ShipType : short
        {
            OneDeck = 1,
            TwoDeck = 2,
            ThreeDeck = 3,
            FourDeck = 4
        }

        public ShipOrientation Orientation { get; set; }
        public ShipType Type { get; set; }
        public Coordinate StartingPoint { get; set; }
    }
}