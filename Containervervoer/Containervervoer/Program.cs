using ContainervervoerLibrary;
using System;
using System.Text;

namespace containervervoer
{
    class Program
    {
        static void Main(string[] args){
            Ship ship = new Ship(4, 2);

            List<Container> containers = [];
            
            //containers.Add(new Container(30000, false, false));
            //containers.Add(new Container(30000, true, false));
            //containers.Add(new Container(30000, false, true));
            //containers.Add(new Container(30000, true, true));


            containers.Add(new Container(30000, true, false));
            containers.Add(new Container(30000, true, false));
            containers.Add(new Container(30000, true, false));
            containers.Add(new Container(30000, true, false));
            containers.Add(new Container(30000, false, false));
            containers.Add(new Container(30000, false, false));
            containers.Add(new Container(30000, false, false));
            containers.Add(new Container(30000, false, false));
            containers.Add(new Container(30000, false, false));
           

            ship.DistributeContainers(containers);
            ship.DisplayContainerPlacement();
            ship.IsMinimumWeightReached(containers);
            ship.IsBalanced();
            var url = $"https://i872272.luna.fhict.nl/ContainerVisualizer/index.html?width={ship.Width}&length={ship.Length}&stacks={TransposeShipLayout(ship)}&weights={GetContainerWeights(ship)}";
            Console.WriteLine(url);
        }
        public static string TransposeShipLayout(Ship ship)
        {
            var maxStacks = ship.Rows.Max(r => r.Stacks.Count);
            var transposed = new List<List<Stack>>(maxStacks);

            for (int i = 0; i < maxStacks; i++)
            {
                transposed.Add(new List<Stack>());
                foreach (var row in ship.Rows)
                {
                    if (i < row.Stacks.Count)
                    {
                        transposed[i].Add(row.Stacks[i]);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var row in transposed)
            {
                foreach (var stack in row)
                {
                    sb.Append(stack.ToString());
                    sb.Append(",");
                }
                if (sb.Length > 0 && sb[sb.Length - 1] == ',')
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("/");
            }
            if (sb.Length > 0 && sb[sb.Length - 1] == '/')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
        
        public static string GetContainerWeights(Ship ship)
        {
            var maxStacks = ship.Rows.Max(r => r.Stacks.Count);
            var transposed = new List<List<Stack>>(maxStacks);

            for (int i = 0; i < maxStacks; i++)
            {
                transposed.Add(new List<Stack>());
                foreach (var row in ship.Rows)
                {
                    if (i < row.Stacks.Count)
                    {
                        transposed[i].Add(row.Stacks[i]);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var row in transposed)
            {
                foreach (var stack in row)
                {
                    for (int i = 0; i < stack.Containers.Count; i++)
                    {
                        sb.Append(stack.Containers[i].ToWeightString());
                        if (i != stack.Containers.Count - 1)
                        {
                            sb.Append("-");
                        }
                    }
                    sb.Append(",");
                }
                if (sb.Length > 0 && sb[sb.Length - 1] == ',')
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("/");
            }
            if (sb.Length > 0 && sb[sb.Length - 1] == '/')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

    }
}


