namespace ContainervervoerLibrary;

public class Row
{
    public Row(int length)
    {

        for (int i = 0; i < length; i++)
        {
            Stacks.Add(new Stack());
        }
    }
    
    public List<Stack> Stacks = [];
}