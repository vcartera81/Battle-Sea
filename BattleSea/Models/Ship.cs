using System;
using System.Collections.Generic;

namespace BattleSea.Models
{
    public class Ship
    {
        private readonly ShipType _shipType;
        private int _decksDestroyed = 0;

        public Ship()
        {
            Id = Guid.NewGuid();
        }

        public Ship(ShipType shipType) : this()
        {
            _shipType = shipType;
        }

        public Ship(ShipType shipType, ShipOrientation shipOrientation, Coordinate startingPoint)
            : this(shipType)
        {
            ShipOrientation = shipOrientation;
            StartingPoint = startingPoint;
        }

        public Guid Id { get; }

        public ShipOrientation ShipOrientation { get; set; }
        public ShipType ShipType => _shipType;
        public ShipState ShipState
        {
            get
            {
                if (_decksDestroyed == 0)
                    return ShipState.Clean;

                return _decksDestroyed < (int)_shipType 
                    ? ShipState.Wounded 
                    : ShipState.Destroyed;
            }
        }

        public Coordinate StartingPoint { get; set; }

        public void SetRandomOrientation(Random random)
        {
            var values = Enum.GetValues(typeof(ShipOrientation));
            ShipOrientation = (ShipOrientation)values.GetValue(random.Next(values.Length));
        }

        public void Shot()
        {
            if (_decksDestroyed < (int) _shipType)
                _decksDestroyed++;
        }

        public IEnumerable<Coordinate> GetCoordinates()
        {
            var startingPoint = Coordinate.Copy(StartingPoint);
            var coordinates = new List<Coordinate>((int) _shipType);
            for (var i = 0; i < (int)_shipType; i++)
            {
                coordinates.Add(Coordinate.Copy(startingPoint));
                if (ShipOrientation == ShipOrientation.Horizontal)
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