namespace ContainervervoerLibrary;

public class Ship
{
    public List<Row> Rows { get; } = [];
    public List<Container> NotFittedContainers = [];
    public int Weight;
    // private int LeftWeight;
    // private int RightWeight;
    public bool Balanced;

    public Ship(int length, int width) {

        for (int i = 0; i < length; i++) Rows.Add(new Row(width, i));
    }
    
    public void DistributeContainers(List<Container> containers)
    {
        List<Container> ordered = containers
            .OrderBy(c => c.Cooled)
            .ThenBy(c => c.Valuable)
            .ThenBy(c => c.Weight).ToList();
        
        foreach (Container container in ordered)
        {

            if (!TryAddContainer(this, container))
            {
                NotFittedContainers.Add(container);
            }
        }
        //laat verdeling zien
    }

    private bool TryAddContainer(Ship ship, Container container)
    {
        bool isFitted = false;
        List<Row> rows = ship.Rows;

        foreach (Row row in rows)
        { 
            if (container.Cooled && !row.HasPower) continue;
            
            foreach(Stack stack in row.Stacks.OrderBy(s => s.weight))
            {
                if(!stack.TryAddContainer(container)) continue;
                
                ship.Weight += container.Weight;
                isFitted = true;
                break;
            }
            if(isFitted) break;
        }

        return isFitted;
    }
}