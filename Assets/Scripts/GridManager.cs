using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public LayerMask nontraversableMask;
    public Transform seeker;
    public Transform target;
    public Vector2 gridWorldSize = new Vector2(50.0f, 50.0f);
    public float nodeRadius;

    public Node [,] grid;
    public List<Node> path;
    float nodeSize;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeSize = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeSize);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeSize);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeX];
        Vector3 bottomLeft = getGridCorners(0);

        for(int i = 0;i < gridSizeX; i++)
        {
            for(int j = 0;j < gridSizeY; j++)
            {
                Vector3 worldPos = GirdPosToWorldPos(i, j);
                bool traversable = !(Physics.CheckSphere(worldPos, nodeRadius, nontraversableMask));
                grid[i, j] = new Node(worldPos, new Vector2(i,j) ,traversable);
            }
        }
    }

    /**
        Anti-clock wise ordering
        3---2
        |   |
        0---1

        0: Bottom Left
        1: Bottom Right
        2: Top Right
        3: Top Left

        Defaults to returning the bottom left
    */
    Vector3 getGridCorners(int corner)
    {
        Vector3 cornerPosition = new Vector3(0, 0, 0);

        switch(corner)
        {
            case 0:
                cornerPosition = transform.position - new Vector3(gridWorldSize.x / 2.0f, 0, 0) - new Vector3(0, 0, gridWorldSize.y / 2.0f);
                break;
            case 1:
                cornerPosition = transform.position + new Vector3(gridWorldSize.x / 2.0f, 0, 0) - new Vector3(0, 0, gridWorldSize.y / 2.0f);
                break;
            case 2:
                cornerPosition = transform.position + new Vector3(gridWorldSize.x / 2.0f, 0, 0) + new Vector3(0, 0, gridWorldSize.y / 2.0f);
                break;
            case 3:
                cornerPosition = transform.position - new Vector3(gridWorldSize.x / 2.0f, 0, 0) + new Vector3(0, 0, gridWorldSize.y / 2.0f);
                break;
            default:
                cornerPosition = transform.position - new Vector3(gridWorldSize.x / 2.0f, 0, 0) - new Vector3(0, 0, gridWorldSize.y / 2.0f);
                break;
        }

        return cornerPosition;
    }
    
    /**
        Converts a given world position into a grid position,
        and returns the node at that grid pos
    */
    public Node WorldPosToGridPos(Vector3 worldPos)
    {
        float xPos = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float yPos = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int x =  Mathf.RoundToInt((gridSizeX - 1) * xPos);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPos);

        return grid[x, y];
    }

    /**
        Converts a given grid position into a world position
    */
    public Vector3 GirdPosToWorldPos(int x, int y)
    {
        Vector3 worldPos = getGridCorners(0) + new Vector3(x * nodeSize + nodeRadius, 0, 0)  + new Vector3(0, 0, y * nodeSize + nodeRadius);
        return worldPos;
    }

    public List<Node> getNeighbouringNodes(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                    continue;
                
                int checkX = (int) node.gridPos.x + x;
                int checkY = (int) node.gridPos.y + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        } 

        return neighbours;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1.0f, gridWorldSize.y));
        if(grid != null)
        {
            Node seekerNode = WorldPosToGridPos(seeker.position);
            Node targertNode = WorldPosToGridPos(target.position);

            foreach(Node n in grid)
            {
                Gizmos.color = n.traversable ? Color.white : Color.black;
                if (path != null) 
                {
                    if (path.Contains(n))
                        Gizmos.color = Color.black;
                }

                if (seekerNode == n)
                    Gizmos.color = Color.blue;
                if(targertNode == n)
                    Gizmos.color = Color.green;

                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeSize - 0.1f));
            }
        }
    }

}
