# Pathfinding Algorithms

  A series of pathfinding algorithms visualised using unity, most pathfinding algorthms follow a general format the only thing that differs is the queueing function for what node to visit next.

## A*

![](/video/Pathfinder.gif)

  The first algorithm immplemented is A*, nodes for the algorithm are generated at runtime. The queueing function is ordered by the sum of node traversal cost (f cost) and heuristic cost (h cost), by altering this queueing function other pathfinding algorithms can be rapidly immplemented.