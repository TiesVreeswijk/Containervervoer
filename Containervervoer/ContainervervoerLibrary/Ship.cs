namespace ContainervervoerLibrary;

public class Ship
{
    public List<Row> Rows { get; } = [];
    public List<Container> NotFittedContainers = [];
    public int Weight;
    public int LeftWeight;
    public int RightWeight;
    public bool Balanced;

    public enum Side
    {
        Left,
        Right,
        Middle
    }

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

    public Side GetBestPlacementSide()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            for (int j = 0; j < Rows[i].Stacks.Count; j++)
            {
                int stackWeight = Rows[i].Stacks[j].Containers.Sum(c => c.Weight);


                if (j < Rows[i].Stacks.Count / 2 && Rows.Count % 2 == 0)
                {
                    LeftWeight += stackWeight;
                }
                else if (j > Rows[i].Stacks.Count / 2 && Rows.Count % 2 == 0)
                {
                    RightWeight += stackWeight;
                }
            }
        }

        if (LeftWeight < RightWeight)
        {
            return Side.Left;
        }
        if (RightWeight < LeftWeight)
        {
            return Side.Right;
        }

            return Side.Middle;
        
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
    }

    public bool TryAddContainer(Ship ship, Container container)
    {
        Side side = ship.GetBestPlacementSide();
        Side otherSide = side == Side.Left ? Side.Right : Side.Left;

        if (TryAddContainerToSide(ship, container, side))
        {
            ship.Weight += container.Weight;
            return true;
        }
        if (TryAddContainerToSide(ship, container, otherSide))
        {
            ship.Weight += container.Weight;
            return true;
        }

        return false;
    }

    public bool TryAddContainerToSide(Ship ship, Container container, Side side)
    {
        foreach (var row in ship.Rows)
        {
            // Determine the start and end indices of the stacks to iterate over, depending on the side
            int startIndex = side == Side.Right ? row.Stacks.Count / 2 : 0;
            int endIndex = side == Side.Left ? row.Stacks.Count / 2 : row.Stacks.Count;

            // Order the stacks by the total weight of their containers
            var orderedStacks = row.Stacks
                .Skip(startIndex)
                .Take(endIndex - startIndex)
                .OrderBy(s => s.Containers.Sum(c => c.Weight))
                .ToList();

            foreach (Stack stack in orderedStacks)
            {
                if (container.Cooled && !stack.ParentRow.HasPower) continue;

                if (stack.TryAddContainer(ship, container))
                {
                    return true;
                }
            }
        }

        return false;
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
        if(maxWeight / 2 > containers.Sum(c => c.Weight)) Console.WriteLine("The minimum weight is not reached");
        else Console.WriteLine("The minimum weight is reached");
        
    }
    
    public void IsBalanced()
    { 
        int totalWeight = LeftWeight + RightWeight;

        int weightDifference = LeftWeight - RightWeight;

        if(weightDifference <= 0.2 * totalWeight) Console.WriteLine("The ship is balanced");
        else Console.WriteLine("The ship is not balanced");
    }
}