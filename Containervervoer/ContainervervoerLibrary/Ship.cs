namespace ContainervervoerLibrary;

public class Ship
{
    public List<Row> Rows { get; } = [];
    public List<Container> NotFittedContainers = [];
    public int Weight;
    public int RightWeight;
    public int LeftWeight;
    
    public Ship(int length, int width)
    {
        for (int i = 0; i < length; i++)
        {
            Row row = new Row(width, i);
            row.ParentShip = this;
            Rows.Add(row);
        }
    }

    public void DistributeContainers(List<Container> containers)
    {
        List<Container> ordered = containers
            .OrderByDescending(c => c.Cooled)
            .ThenByDescending(c => c.Valuable)
            .ThenBy(c => c.Weight).ToList();

        foreach (Container container in ordered)
        {
            if (!TryAddContainer(this, container))
            {
                NotFittedContainers.Add(container);
            }
        }
    }
    public void DisplayContainerPlacement()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            Console.WriteLine($"Row {i + 1}:");
            for (int j = 0; j < Rows[i].Stacks.Count; j++)
            {
                Console.WriteLine($"\tStack {j + 1}: {Rows[i].Stacks[j].Containers.Count} containers");
                foreach (var container in Rows[i].Stacks[j].Containers)
                {
                    string containerType = container.Cooled ? "Cooled" : container.Valuable ? "Valuable" : "Normal";
                    Console.WriteLine($"\t\tContainer: Weight = {container.Weight}, Type = {containerType}");
                }
            }
        }

        foreach (var container in NotFittedContainers)
        {
            string containerType = container.Cooled ? "Cooled" : container.Valuable ? "Valuable" : "Normal";
            Console.WriteLine(
                $"Container: Weight = {container.Weight}, Type = {containerType} could not be placed");
        }
    }

    private bool TryAddContainer(Ship ship, Container container)
    {
        foreach (var row in ship.Rows)
        {
            List<Stack> validStacks = ship.getValidStacks(container, ship, row);
            foreach (var stack in validStacks)
            {
                if (stack.TryAddContainer(ship, container))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private List<Stack> getValidStacks(Container container, Ship ship, Row row)
    {
        int halfRow = row.Stacks.Count / 2;
        bool isRowEven = row.Stacks.Count % 2 == 0;

        bool addToFirstHalf = ship.LeftWeight <= ship.RightWeight;
        bool addToLastHalf = ship.LeftWeight > ship.RightWeight;

        List<Stack> selectedStacks = new List<Stack>();

        if (!isRowEven && ship.LeftWeight == ship.RightWeight &&
            row.Stacks[halfRow].Weight + container.Weight <= Container.MaxWeightAbove &&
            !(row.Stacks[halfRow].TopContainerIsValuable() && container.Valuable ||
              (container.Valuable && container.Cooled)))
        {
            selectedStacks.Add(row.Stacks[halfRow]);
        }
        else
        {
            var firstHalfStacks = row.Stacks.Take(halfRow).Where(stack =>
                !(stack.TopContainerIsValuable() && container.Valuable) &&
                !(container.Valuable && stack.ContainsValuableContainer())).ToList();
            var lastHalfStacks = row.Stacks.Skip(isRowEven ? halfRow : halfRow + 1).Where(stack =>
                !(stack.TopContainerIsValuable() && container.Valuable) &&
                !(container.Valuable && stack.ContainsValuableContainer())).ToList();

            if (addToFirstHalf)
            {
                selectedStacks.AddRange(firstHalfStacks);
                LeftWeight += container.Weight;
            }
            else if (addToLastHalf)
            {
                selectedStacks.AddRange(lastHalfStacks);
                RightWeight += container.Weight;
            }
            else
            {
                // Als de middelste stack niet kan worden gebruikt, voeg dan de container toe aan de linkerkant
                if (selectedStacks.Count == 0)
                {
                    selectedStacks.AddRange(firstHalfStacks);
                }
                else
                {
                    selectedStacks.AddRange(firstHalfStacks);
                }
            }
        }

        return selectedStacks;
    }
    public void IsBalanced()
    { 
        int totalWeight = LeftWeight + RightWeight;

        int weightDifference = LeftWeight - RightWeight;

        if(weightDifference <= 0.2 * totalWeight) Console.WriteLine("The ship is balanced");
        else Console.WriteLine("The ship is not balanced");
    }
    public void IsMinimumWeightReached(List<Container> containers)
    {
        int maxWeight = 0;
        int usedWeight = 0;
        foreach(Row row in Rows)
        {
            foreach(Stack stack in row.Stacks)
            {
                maxWeight += Container.MaxWeight + Container.MaxWeightAbove;
            }
        }
        foreach(Container container in containers)
        {
            usedWeight += container.Weight;
        }
        if(maxWeight / 2 > usedWeight) Console.WriteLine("The minimum weight is not reached");
        else Console.WriteLine("The minimum weight is reached");
    
    }
}




