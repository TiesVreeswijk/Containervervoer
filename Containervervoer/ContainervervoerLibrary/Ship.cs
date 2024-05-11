namespace ContainervervoerLibrary;

public class Ship
{
    public List<Row> Rows { get; } = [];
    public int Weight;
    private int LeftWeight;
    private int RightWeight;
    public bool Balanced;
    
    public Ship(int length, int width) {

        for (int i = 0; i < length; i++) Rows.Add(new Row(width));
    }
    
    public void DistributeContainers(List<Container> containers)
    {
        
    }
}