using BattleSea.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleSeaTest
{
    [TestClass]
    public class CoordinateTest
    {
        [TestMethod]
        public void AreEqualIfRowAndColumnMatch()
        {
            var coordinate1 = new Coordinate('c', 2);
            var coordinate2 = new Coordinate('c', 2);

            Assert.IsTrue(coordinate1.Equals(coordinate2));
        }

        [TestMethod]
        public void CanCopyTwoCoordinates()
        {
            var coordinate = new Coordinate('b', 7);
            var copy = Coordinate.Copy(coordinate);

            Assert.IsTrue(coordinate.Equals(copy));
        }

        [TestMethod]
        public void CanMoveCoordinateToAnyDirection()
        {
            var coordinate = new Coordinate('e', 5);

            coordinate.MoveUp();
            Assert.IsTrue(coordinate.Equals(new Coordinate('e', 4)));

            coordinate.MoveRight();
            Assert.IsTrue(coordinate.Equals(new Coordinate('f', 4)));

            coordinate.MoveDown();
            Assert.IsTrue(coordinate.Equals(new Coordinate('f', 5)));

            coordinate.MoveLeft();
            Assert.IsTrue(coordinate.Equals(new Coordinate('e', 5)));
        }
    }
}
