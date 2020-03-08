using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool traversable { get; set; }
    public Vector3 worldPos { get; set; }
    public Vector2 gridPos { get; set; }
    public float cost {get; set;}
    public Node parent;
    public float health = 100.0f;

    public Node(Vector3 _worldPos, Vector2 _gridPos, bool _traversable)
    {
        this.worldPos = _worldPos;
        this.gridPos = _gridPos;
        this.traversable = _traversable;
    }
    
    public override string ToString()
    {
        return "World Position: " + this.worldPos + " Grid Position: " + this.gridPos + " Traversable: " + this.traversable;
    }
}
