using ContainervervoerLibrary;

Ship ship = new Ship(3,2);

List<Container> containers = [];

containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, false, false));
containers.Add(new Container(30000, true, false));
containers.Add(new Container(30000, true, false));
containers.Add(new Container(30000, true, false));
containers.Add(new Container(30000, true, false));


ship.DistributeContainers(containers);
ship.DisplayContainerPlacement();