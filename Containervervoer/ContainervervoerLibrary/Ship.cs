using System.Collections.ObjectModel;

namespace ContainervervoerLibrary;

public class Ship
{
    private List<Row> _rows { get; } = [];
    public ReadOnlyCollection<Row> Rows => _rows.AsReadOnly();
    private readonly List<Container> _notFittedContainers = [];
    
    
    private int _rightWeight;
    private int _leftWeight;
    public readonly int Length;
    public readonly int Width;
    private int MaxWeight
    {
        get
        {
            int totalWeight = 0;
            foreach (var row in _rows)
            {
                foreach (var stack in row.Stacks)
                {
                    totalWeight += Container.MaxWeight + Container.MaxWeightAbove;
                }
            }
            return totalWeight;
        }
    }
    public int MaxWeightDifference => (int) (MaxWeight * 0.2);
    
    public Ship(int length, int width)
    {
        Length = length;
        Width = width;
        for (int i = 0; i < length; i++)
        {
            Row row = new Row(width, i)
            {
                Ship = this
            };
            _rows.Add(row);
        }
    }
    
    public override string ToString()
    {
        var output = "";
        for (int i = 0; i < Rows.Count; i++)
        {
            output += _rows[i].ToString();
            if (i != Rows.Count - 1)
            {
                output += "/";
            }
        }

        return output;
    }


    public void DistributeContainers(List<Container> containers)
    {
        List<Container> ordered = containers
            .OrderByDescending(c => c.Cooled)
            .ThenByDescending(c => c.Valuable)
            .ThenBy(c => c.Weight).ToList();

        foreach (Container container in ordered)
        {
            if (TryAddContainer(this, container)) container.Placed = true;
        }

        foreach (Container container in containers)
        {
            if(!container.Placed) if(TryAddContainer(this, container)) container.Placed = true;
        }
        foreach(Container container in containers)
        {
            if(!container.Placed) _notFittedContainers.Add(container);
        }

    }
    

    public void DisplayContainerPlacement()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            Console.WriteLine($"Row {i + 1}:");
            for (int j = 0; j < _rows[i].Stacks.Count; j++)
            {
                Console.WriteLine($"\tStack {j + 1}: {_rows[i].Stacks[j].Containers.Count} containers");
                foreach (var container in _rows[i].Stacks[j].Containers)
                {
                    string containerType = container.Cooled ? "Cooled" : container.Valuable ? "Valuable" : "Normal";
                    Console.WriteLine($"\t\tContainer: Weight = {container.Weight}, Type = {containerType}");
                }
            }
        }

        foreach (var container in _notFittedContainers)
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
            List<Stack> validStacks = ship.GetValidStacks(container, ship, row);
            bool addToLeft = ship._leftWeight <= ship._rightWeight;
            bool addToMiddle = row.Stacks.Count % 2 != 0 && row.Stacks.IndexOf(row.Stack) == row.Stacks.Count / 2;
            foreach (var stack in validStacks)
            {
                if (stack.TryAddContainer(ship, container))
                {
                    if (!addToMiddle)
                    {
                        if (addToLeft) ship._leftWeight += container.Weight;
                        else ship._rightWeight += container.Weight;  
                    }
                    return true;
                }
            }
        }

        return false;
    }

    private List<Stack> GetValidStacks(Container container, Ship ship, Row row)
    {
        int halfRow = row.Stacks.Count / 2;
        bool isRowEven = row.Stacks.Count % 2 == 0;

        bool addToFirstHalf = ship._leftWeight <= ship._rightWeight;
        bool addToLastHalf = ship._leftWeight > ship._rightWeight;

        List<Stack> selectedStacks = new List<Stack>();

        if (!isRowEven && ship._leftWeight == ship._rightWeight &&
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

            if (addToFirstHalf && firstHalfStacks.Count != 0)
            {
                selectedStacks.AddRange(firstHalfStacks);
                
            }
            else if (addToLastHalf && lastHalfStacks.Count != 0)
            {
                selectedStacks.AddRange(lastHalfStacks);
                
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
    public bool IsBalanced()
    { 
        int totalWeight = _leftWeight + _rightWeight;

        int weightDifference = _leftWeight - _rightWeight;

        return (weightDifference <= 0.2 * totalWeight) ;
    }
    public bool IsMinimumWeightReached()
    {
        int maxWeight = MaxWeight;
        int usedWeight = 0;
        foreach(var row in Rows)
        {
            foreach(var stack in row.Stacks)
            {
                foreach(var container in stack.Containers)
                {
                    usedWeight += container.Weight;
                }
            }
        }

        return (maxWeight / 2 < usedWeight);


    }
}




