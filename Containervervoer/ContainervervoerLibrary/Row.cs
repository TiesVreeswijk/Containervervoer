namespace ContainervervoerLibrary;

public class Row
{
    public int Index { get; }
    public List<Stack> Stacks = [];

    public Row(int length, int index)
    {
        Index = index;

        for (int i = 0; i < length; i++)
        {
            Stacks.Add(new Stack());
        }
    }

    public bool HasPower => Index == 0;
}