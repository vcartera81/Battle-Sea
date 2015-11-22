using System;

namespace BattleSea.Models
{
    public class Ship
    {
        public Ship()
        {
        }

        public Ship(Type type)
        {
            Type = type;
        }

        public Orientation Orientation { get; set; }
        public Type Type { get; set; }
        public Coordinate StartingPoint { get; set; }

        public void SetRandomOrientation(Random random)
        {
            var values = Enum.GetValues(typeof(Orientation));
            Orientation = (Orientation)values.GetValue(random.Next(values.Length));
        }
    }

    public enum Orientation
    {
        Vertical,
        Horizontal
    }

    public enum Type : short
    {
        OneDeck = 1,
        TwoDeck = 2,
        ThreeDeck = 3,
        FourDeck = 4
    }
}