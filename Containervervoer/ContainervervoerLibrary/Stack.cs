namespace ContainervervoerLibrary;

public class Stack
{
    public int Weight;

    public Row ParentRow { get; set; }
    public List<Container> Containers;

    public Stack()
    {
        Containers = new List<Container>();
    }

    public bool TryAddContainer(Ship ship, Container container)
    {
        if (BlocksValuable()) return false;

        if (container.Valuable)
        {
            if (Containers.Any(c => c.Valuable) || TooMuchWeightAbove(container.Weight, 1) || !CanAcces())
            {
                return false;
            }

            Containers.Add(container);
            return true;
        }

        if (!TooMuchWeightAbove(0, 0) &&
            (Containers.Count == 0 || container.Weight > Containers[0].Weight || Containers[0].Valuable))
        {
            Containers.Insert(0, container);
            return true;
        }

        if (TooMuchWeightAbove(container.Weight, 1)) return false;

        for (int i = 1; i <= Containers.Count; i++)
        {
            if (i < Containers.Count && !Containers[i].Valuable && Containers[i].Weight > container.Weight) continue;

            Containers.Insert(i, container);
            return true;
        }

        return false;
    }

    private bool TooMuchWeightAbove(int weightToAdd, int skip)
    {
        if (Containers.Skip(skip).Sum(c => c.Weight) + weightToAdd > Container.MaxWeightAbove) return true;
        return false;
    }

    private bool BlocksValuable()
    {
        var currentRowIndex = ParentRow.ParentShip.Rows.IndexOf(ParentRow);
        var currentStackIndex = ParentRow.Stacks.IndexOf(this);


        var stackBefore = currentRowIndex > 0
            ? ParentRow.ParentShip.Rows[currentRowIndex - 1].Stacks[currentStackIndex]
            : null;
        var stackTwoBefore = currentRowIndex > 1
            ? ParentRow.ParentShip.Rows[currentRowIndex - 2].Stacks[currentStackIndex]
            : null;

        var stackAfter = currentRowIndex < ParentRow.ParentShip.Rows.Count - 1
            ? ParentRow.ParentShip.Rows[currentRowIndex + 1].Stacks[currentStackIndex]
            : null;
        var stackTwoAfter = currentRowIndex < ParentRow.ParentShip.Rows.Count - 2
            ? ParentRow.ParentShip.Rows[currentRowIndex + 2].Stacks[currentStackIndex]
            : null;

        if (stackBefore?.Containers.LastOrDefault(c => c.Valuable) != null &&
            stackTwoBefore != null &&
            stackTwoBefore.Containers.Count >= stackBefore.Containers.Count &&
            Containers.Count + 1 >= stackBefore.Containers.Count) return true;

        if (stackAfter?.Containers.LastOrDefault(c => c.Valuable) != null &&
            stackTwoAfter != null &&
            stackTwoAfter.Containers.Count >= stackAfter.Containers.Count &&
            Containers.Count + 1 >= stackAfter.Containers.Count) return true;

        return false;
    }

    private bool CanAcces()
    {
        int currentRowIndex = ParentRow.ParentShip.Rows.IndexOf(ParentRow);
        int currentStackIndex = ParentRow.Stacks.IndexOf(this);

        if (currentRowIndex == 0 ||
            currentRowIndex == ParentRow.ParentShip.Rows.Count - 1 ||
            ParentRow.ParentShip.Rows[currentRowIndex - 1].Stacks[currentStackIndex].Containers.Count <=
            Containers.Count ||
            ParentRow.ParentShip.Rows[currentRowIndex + 1].Stacks[currentStackIndex].Containers.Count <=
            Containers.Count) return true;

        return false;
    }
}