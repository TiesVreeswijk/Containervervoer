using ContainervervoerLibrary;

namespace ContainerVervoerTests;

public class StackTests
{
     [Test]
        public void ContainsValuableContainer_ShouldReturnExpectedResult()
        {
            // Arrange
            Ship ship = new Ship(2, 2);

            var container1 = new Container(30000, true, false);
            var container2 = new Container(15000, false, false);
            ship.Rows[0].Stacks[0].TryAddContainer(ship, container1);
            ship.Rows[0].Stacks[0].TryAddContainer(ship, container2);

            // Act
            var result = ship.Rows[0].Stacks[0].ContainsValuableContainer();

            // Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public void TryAddContainer_ShouldAddContainerToStack()
        {
            // Arrange
            var ship = new Ship(2, 2);
            var container = new Container(30000, false, false);

            // Act
            ship.Rows[0].Stacks[0].TryAddContainer(ship, container);

            // Assert
            Assert.Contains(container, ship.Rows[0].Stacks[0].Containers);
        }

        [Test]
        public void TopContainerIsValuable_ShouldReturnExpectedResult()
        {
            // Arrange
            Ship ship = new Ship(2, 2);

            var container1 = new Container(30000, true, false);
            var container2 = new Container(15000, false, false);
            ship.Rows[0].Stacks[0].TryAddContainer(ship, container1);
            ship.Rows[0].Stacks[0].TryAddContainer(ship, container2);
            

            // Act
            var result = ship.Rows[0].Stacks[0].TopContainerIsValuable();

            // Assert
            Assert.IsTrue(result);
        }
}