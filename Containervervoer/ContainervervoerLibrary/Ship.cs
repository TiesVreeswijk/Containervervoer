namespace ContainervervoerLibrary;

public class Ship
{
    public List<Row> Rows { get; } = [];
    public List<Container> NotFittedContainers = [];
    public int Weight;
    public int LeftWeight;
    public int RightWeight;
    public bool Balanced;
    public Side LastPlacementSide { get; private set; } = Side.Right;

    public Ship(int length, int width) {

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
    }

    public enum Side { Left, Right, Middle }

    public Side GetPlacementSide(Container container)
    {
        // Alternate between placing containers on the left and right sides
        if (LastPlacementSide == Side.Middle && RightWeight < LeftWeight)
        {
            LastPlacementSide = Side.Right;
        }
        else if (LastPlacementSide == Side.Middle && LeftWeight < RightWeight)
        {
            LastPlacementSide = Side.Left;
        }
        else if (LeftWeight == RightWeight)
        {
            LastPlacementSide = Side.Middle;
        }
        else
        {
            LastPlacementSide = LastPlacementSide == Side.Left ? Side.Right : Side.Left;
        }
        
        return LastPlacementSide;
    }
    
    private bool TryAddContainer(Ship ship, Container container)
    {
        bool isFitted = false;
        Side side = ship.GetPlacementSide(container);

        foreach (var row in ship.Rows)
        {
            Stack stack;
            if (side == Side.Middle && row.Stacks.Count % 2 != 0)
            {
                stack = row.Stacks[row.Stacks.Count / 2];
            }
            else
            {
                stack = side == Side.Left ? row.Stacks.First() : row.Stacks.Last();
            }

            if (container.Cooled && !stack.ParentRow.HasPower) continue;

            if(stack.TryAddContainer(ship, container))
            {
                ship.Weight += container.Weight;
                isFitted = true;
                break;
            }
        }

        return isFitted;
    }
}