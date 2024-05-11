using ContainervervoerLibrary;

Ship ship = new Ship(2,2);

List<Container> containers = [];

containers.Add(new Container(30000, false, false));
containers.Add(new Container(15000, false, false));

ship.DistributeContainers(containers);