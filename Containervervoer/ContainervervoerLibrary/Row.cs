namespace ContainervervoerLibrary;

public class Row
{
    public Ship ParentShip { get; set; }
    public int Index { get; }
    public List<Stack> Stacks = [];
    

    public Row(int length, int index)
    {
        Index = index;

        for (int i = 0; i < length; i++)
        {
            Stack stack = new Stack();
            stack.ParentRow = this;
            Stacks.Add(stack);
        }
    }
    public bool HasPower => Index == 0;
    public Stack Stack { get; set; }
}