﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pathfinding based on Sebasian Lague's A* Pathfinding series

public class Node : IHeapItem<Node>
{

    public bool driveable;
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;

    public int gridX;
    public int gridY;

    public Node parent;
    int heapIndex;

    public Node(bool _driveable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        driveable = _driveable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
       get { return gCost + hCost; }
    }

    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
