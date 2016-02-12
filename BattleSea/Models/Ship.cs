using System;
using System.Collections.Generic;

namespace BattleSea.Models
{
    public class Ship
    {
        private int _decksDestroyed = 0;

        public Ship()
        {
            Id = Guid.NewGuid();
        }

        public Ship(ShipType type) : this()
        {
            Type = type;
        }

        public Ship(ShipType type, ShipOrientation orientation, Coordinate startingPoint)
            : this(type)
        {
            Orientation = orientation;
            StartingPoint = startingPoint;
        }

        public Guid Id { get; }

        public ShipOrientation Orientation { get; set; }
        public ShipType Type { get; }

        public ShipState State
        {
            get
            {
                if (_decksDestroyed == 0)
                    return ShipState.Clean;

                return _decksDestroyed < (int)Type 
                    ? ShipState.Wounded 
                    : ShipState.Destroyed;
            }
        }

        public Coordinate StartingPoint { get; set; }

        public void SetRandomOrientation(Random random)
        {
            var values = Enum.GetValues(typeof(ShipOrientation));
            Orientation = (ShipOrientation)values.GetValue(random.Next(values.Length));
        }

        public void Shot()
        {
            if (_decksDestroyed < (int) Type)
                _decksDestroyed++;
        }

        public IEnumerable<Coordinate> GetCoordinates()
        {
            var startingPoint = Coordinate.Copy(StartingPoint);
            var coordinates = new List<Coordinate>((int) Type);
            for (var i = 0; i < (int)Type; i++)
            {
                coordinates.Add(Coordinate.Copy(startingPoint));
                if (Orientation == ShipOrientation.Horizontal)
                    startingPoint.MoveRight();
                else
                    startingPoint.MoveDown();
            }
            return coordinates;
        } 
    }

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

    public enum ShipState
    {
        Clean,
        Wounded,
        Destroyed
    }
}