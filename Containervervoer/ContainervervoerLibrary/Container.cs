namespace ContainervervoerLibrary;

public class Container() {
    public const int MinWeight = 4000;
    public const int MaxWeight = 30000;
    public const int MaxWeightAbove = 120000;
    public readonly int Weight;
    public readonly bool Valuable;
    public readonly bool Cooled;

    public Container(int weight, bool valuable, bool cooled) : this()
    {
        Weight = weight;
        Valuable = valuable;
        Cooled = cooled;
    }

    public bool Placed { get; set; }

    public string ToWeightString()
    {
        return (Weight/1000).ToString();
    }
        
    public override string ToString()
    {
        string containerType = "1";
        if (Valuable && Cooled)
        {
            containerType = "4";
        }
        else if (Valuable)
        {
            containerType = "2";
        }
        else if (Cooled)
        {
            containerType = "3";
        }

        return containerType;
    }

}
