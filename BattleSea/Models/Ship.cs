using System;

namespace BattleSea.Models
{
    public class Ship
    {
        private readonly Guid _id;
        private readonly ShipType _shipType;
        //private Coordinate[] _coordinates;
        private int _decksDestroyed = 0;

        public Ship()
        {
            _id = Guid.NewGuid();
        }

        public Ship(ShipType shipType) : this()
        {
            _shipType = shipType;
        }

        public Guid Id => _id;
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