namespace ContainervervoerLibrary;

public class Container() {
    public const int MinWeight = 4000;
    public const int MaxWeight = 30000;
    public const int MaxWeightAbove = 120000;
    public int Weight;
    public bool Valuable;
    public bool Cooled;

    public Container(int weight, bool valuable, bool cooled) : this()
    {
        Weight = weight;
        Valuable = valuable;
        Cooled = cooled;
    }
}
