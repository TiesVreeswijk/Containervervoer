using ContainervervoerLibrary;
using System.IO;

namespace ContainerVervoerTests
{
    public class ShipTests
    {
        [Test]
        public void ShipConstructor_GivenDimensions_ShouldCreateCorrectNumberOfRowsAndStacks()
        {
            // Arrange
            Ship ship = new Ship(2, 4);

            // Act
            int actualRows = ship.Rows.Count;
            int actualStacks = ship.Rows.First().Stacks.Count;

            // Assert
            Assert.That(actualRows, Is.EqualTo(2));
            Assert.That(actualStacks, Is.EqualTo(4));
        }
        [Test]
        public void ShipMaxWeight_GivenShipDimensionsOf2By2_ShouldReturnExpectedMaxWeight()
        {
            // Arrange
            Ship ship = new Ship(2, 2);
            int expectedMaxWeight = 600000;

            // Act
            int actualMaxWeight = 0;
            foreach(Row row in ship.Rows)
            foreach(Stack stack in row.Stacks)
            {
                actualMaxWeight += Container.MaxWeight + Container.MaxWeightAbove;
            }

            // Assert
            Assert.That(actualMaxWeight, Is.EqualTo(expectedMaxWeight));
        }
        
        [Test]
        public void IsBalanced_WithOddWidth_ShouldReturnFalse()
        {
            // Arrange
            Ship ship = new Ship(2, 3);
            Container container1 = new Container(30000, true, false);
            Container container2 = new Container(30000, true, false);
            Container container3 = new Container(30000, true, false);

            // Manually add containers to stacks
            
            ship.Rows[0].Stacks[0].TryAddContainer(ship, container1);
            ship.Rows[0].Stacks[1].TryAddContainer(ship, container2);
            ship.Rows[1].Stacks[0].TryAddContainer(ship, container3);

            // Calculate expected outcome
            int leftSideWeight = ship.Rows[0].Stacks.Sum(stack => stack.Containers.Sum(container => container.Weight));
            int rightSideWeight = ship.Rows[1].Stacks.Sum(stack => stack.Containers.Sum(container => container.Weight));
            bool expectedOutcome = Math.Abs(leftSideWeight - rightSideWeight) <= ship.MaxWeightDifference;


            // Act
            ship.IsBalanced();

            // Assert
             Assert.That(ship.IsBalanced(), Is.EqualTo(expectedOutcome));
        }
        [Test]
        public void IsMinimumWeightReached_WithTooLowWeight_ShouldReturnFalse()
        {
            // Arrange
            Ship ship = new Ship(2, 2);
            Container container1 = new Container(30000, true, false);
            Container container2 = new Container(30000, false, false);
            Container container3 = new Container(30000, false, false);

            // Manually add containers to stacks
            ship.Rows[0].Stacks[0].TryAddContainer(ship, container1);
            ship.Rows[0].Stacks[1].TryAddContainer(ship, container2);
            ship.Rows[1].Stacks[0].TryAddContainer(ship, container3);
            

            // Act
            var result = ship.IsMinimumWeightReached();

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void DistributeContainers_TotalContainersMatchesInput()
        {
            //arrange
            Ship ship = new Ship(2, 2);
            List<Container> containers = new List<Container>
            {
                new Container(30000, true, false),
                new Container(30000, true, false),
                new Container(30000, true, false),
                new Container(30000, false, false),
                new Container(30000, false, false),
                new Container(30000, false, false)
            };
            
            //act
            ship.DistributeContainers(containers);
            int totalContainersOnShip = ship.Rows.Sum(row => row.Stacks.Sum(stack => stack.Containers.Count));
            
            Assert.That(totalContainersOnShip, Is.EqualTo(containers.Count));
        }
    }
}