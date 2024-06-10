using ContainervervoerLibrary;

Ship ship = new Ship(3,5);

List<Container> containers = [];


containers.Add(new Container(30000, true, false));
containers.Add(new Container(30000, true, false));
containers.Add(new Container(30000, true, false));
containers.Add(new Container(30000, true, false));
containers.Add(new Container(30000, false, true));
containers.Add(new Container(30000, false, true));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));


ship.DistributeContainers(containers);
ship.DisplayContainerPlacement();
ship.IsMinimumWeightReached(containers);
ship.IsBalanced();