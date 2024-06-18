using System.Collections.ObjectModel;

namespace ContainervervoerLibrary;

public class Row
{
    public Ship Ship { get; set; }
    public Stack Stack { get; }
    private int Index { get; }
    private List<Stack> _stacks = [];
    public ReadOnlyCollection<Stack> Stacks => _stacks.AsReadOnly();
    

    public Row(int length, int index)
    {
        Index = index;

        for (int i = 0; i < length; i++)
        {
            Stack stack = new Stack();
            stack.Row = this;
            _stacks.Add(stack);
        }
    }
    public override string ToString()
    {
        var output = "";
        for (int i = 0; i < Stacks.Count; i++)
        {
            output += Stacks[i].ToString();
            if (i != Stacks.Count - 1)
            {
                output += ",";
            }
        }

        return output;
    }

    public bool HasPower => Index == 0;
    
}