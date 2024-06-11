using ContainervervoerLibrary;
using System;

Ship ship = new Ship(2,3);

List<Container> containers = [];
//containers.Add(new Container(30000, false, false));
//containers.Add(new Container(30000, true, false));
//containers.Add(new Container(30000, false, true));
//containers.Add(new Container(30000, true, true));


containers.Add(new Container(30000, true, false));
containers.Add(new Container(30000, true, false));
containers.Add(new Container(30000, true, false));


ship.DistributeContainers(containers);
ship.DisplayContainerPlacement();
ship.IsMinimumWeightReached(containers);
ship.IsBalanced();

