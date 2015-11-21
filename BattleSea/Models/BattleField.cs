using System.Collections.Generic;

namespace BattleSea.Models
{
    public class BattleField
    {
        public BattleFieldCells Cells { get; set; }
        public IEnumerable<Ship> Ships { get; set; }

        public BattleField(int size)
        {
            Cells = new BattleFieldCells(size);
        }

        public static IEnumerable<Ship> GetDefaultShips()
        {
            return new[]
            {
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.FourDeck
                },
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.ThreeDeck
                },
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.ThreeDeck
                },
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.TwoDeck
                },
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.TwoDeck
                },
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.TwoDeck
                },
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.OneDeck
                },
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.OneDeck
                },
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.OneDeck
                },
                new Ship
                {
                    Orientation = Ship.ShipOrientation.Horizontal,
                    StartingPoint = default(Coordinate),
                    Type = Ship.ShipType.OneDeck
                }
            };
        }
    }
}