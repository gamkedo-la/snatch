﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{

    public GraphNode nodeA;
    public GraphNode nodeB;
    public GraphPathfinding graphPathfinding;

    public float connectionDistance
    {
        get { return Vector3.Distance(nodeA.worldPosition, nodeB.worldPosition); }
    }

    void OnDrawGizmos()
    {
        if(graphPathfinding.showGridConnections)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(nodeA.gameObject.transform.position, nodeB.gameObject.transform.position);
        }

    }

}
