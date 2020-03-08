using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind : MonoBehaviour
{
    GridManager gridManager;

    // Update is called once per frame
    void Update()
    {
        Node seeker = gridManager.WorldPosToGridPos(gridManager.seeker.position);
        Node target = gridManager.WorldPosToGridPos(gridManager.target.position);

        findPath(seeker, target);
    }

    void findPath(Node startNode, Node endNode)
    {
        List<Node> openNodes = new List<Node>();
        List<Node> closedNodes = new List<Node>();
        openNodes.Add(startNode);

        while(openNodes.Count > 0)
        {
            Node currentNode = openNodes[0];
            //Debug.Log(currentNode.ToString());

            openNodes.Sort((x,y)=>x.cost.CompareTo(y.cost));

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if(currentNode == endNode)
            {
                retrace(startNode, endNode);
                return;
            }

            foreach(Node n in gridManager.getNeighbouringNodes(currentNode))
            {
                if(!n.traversable || closedNodes.Contains(n))
                    continue;

                float movementCost = currentNode.cost + calculateDistance(currentNode, n);

                if(movementCost < n.cost || !openNodes.Contains(n))
                {
                    n.cost = movementCost;
                    n.parent = currentNode;
                    if(!openNodes.Contains(n))
                        openNodes.Add(n);
                }
            }

        }
    }

    void retrace(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        gridManager.path = path;
    }

    /**
        For heuristicts, therefore distance will be world distance 
        instead of node distance.
    */
    float calculateDistance(Node one, Node two)
    {
        return Vector3.Distance(gridManager.GirdPosToWorldPos((int)one.gridPos.x, (int)one.gridPos.y), gridManager.GirdPosToWorldPos((int)two.gridPos.x, (int)two.gridPos.y));
    }

    void Awake() 
    {
        // Get refrence to grid manager object
        gridManager = GetComponent<GridManager>();    
    }
}
