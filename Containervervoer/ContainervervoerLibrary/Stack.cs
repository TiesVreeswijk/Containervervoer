using System.Collections.ObjectModel;

namespace ContainervervoerLibrary;

public class Stack
{
    public int Weight;

    public Row Row { get; set; }
    
    private List<Container> _containers;
    public ReadOnlyCollection<Container> Containers => _containers.AsReadOnly();
    


    public Stack()
    {
        Weight = 0;
        _containers = new List<Container>();
    }

    public override string ToString()
    {
        var output = "";
        for (int i = 0; i < _containers.Count; i++)
        {
            output += _containers[i].ToString();
            if (i != _containers.Count - 1)
            {
                output += "-";
            }
        }

        return output;
    }

    public bool TryAddContainer(Ship ship, Container container)
    {
        
        if (BlocksValuable()) return false;

        if (container.Valuable)
        {
            if (_containers.Any(c => c.Valuable) || TooMuchWeightAbove(container.Weight, 1) || !CanAcces())
            {
                return false;
            }

            _containers.Add(container);
            this.Weight += container.Weight;
            return true;
        }

        if (!TooMuchWeightAbove(0, 0) &&
            (_containers.Count == 0 || container.Weight > _containers[0].Weight || _containers[0].Valuable))
        {
            _containers.Insert(0, container);
            this.Weight += container.Weight;
            return true;
        }

        if (TooMuchWeightAbove(container.Weight, 1)) return false;

        for (int i = 1; i <= _containers.Count; i++)
        {
            if (i < _containers.Count && !_containers[i].Valuable && _containers[i].Weight > container.Weight) continue;

            _containers.Insert(i, container);
            this.Weight += container.Weight;
            return true;
        }

        return false;
    }

    private bool TooMuchWeightAbove(int weightToAdd, int skip)
    {
        return(_containers.Skip(skip).Sum(c => c.Weight) + weightToAdd > Container.MaxWeightAbove);

    }

    private bool BlocksValuable()
    {
        var currentRowIndex = Row.ParentShip.Rows.IndexOf(Row);
        var currentStackIndex = Row.Stacks.IndexOf(this);


        var stackBefore = currentRowIndex > 0
            ? Row.ParentShip.Rows[currentRowIndex - 1].Stacks[currentStackIndex]
            : null;
        var stackTwoBefore = currentRowIndex > 1
            ? Row.ParentShip.Rows[currentRowIndex - 2].Stacks[currentStackIndex]
            : null;

        var stackAfter = currentRowIndex < Row.ParentShip.Rows.Count - 1
            ? Row.ParentShip.Rows[currentRowIndex + 1].Stacks[currentStackIndex]
            : null;
        var stackTwoAfter = currentRowIndex < Row.ParentShip.Rows.Count - 2
            ? Row.ParentShip.Rows[currentRowIndex + 2].Stacks[currentStackIndex]
            : null;

        if (stackBefore?.Containers.LastOrDefault(c => c.Valuable) != null &&
            stackTwoBefore != null &&
            stackTwoBefore.Containers.Count >= stackBefore.Containers.Count &&
            _containers.Count + 1 >= stackBefore.Containers.Count) return true;

        if (stackAfter?.Containers.LastOrDefault(c => c.Valuable) != null &&
            stackTwoAfter != null &&
            stackTwoAfter.Containers.Count >= stackAfter.Containers.Count &&
            _containers.Count + 1 >= stackAfter.Containers.Count) return true;

        return false;
    }

    private bool CanAcces()
    {
        int currentRowIndex = Row.ParentShip.Rows.IndexOf(Row);
        int currentStackIndex = Row.Stacks.IndexOf(this);

        if (currentRowIndex == 0 ||
            currentRowIndex == Row.ParentShip.Rows.Count - 1 ||
            Row.ParentShip.Rows[currentRowIndex - 1].Stacks[currentStackIndex].Containers.Count <=
            _containers.Count ||
            Row.ParentShip.Rows[currentRowIndex + 1].Stacks[currentStackIndex].Containers.Count <=
            _containers.Count) return true;

        return false;
    }

    public bool TopContainerIsValuable()
    {
        return _containers.Count > 0 && _containers.Last().Valuable;
    }
    
    public bool ContainsValuableContainer()
    {
        return _containers.Any(c => c.Valuable);
    }
}