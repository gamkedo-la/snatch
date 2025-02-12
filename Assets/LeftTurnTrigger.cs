﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTurnTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<LeftTurnTagScript>() != null)
        {
            other.transform.Rotate(0, -90, 0);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 1, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(transform.GetComponent<BoxCollider>().size.x, transform.GetComponent<BoxCollider>().size.y, transform.GetComponent<BoxCollider>().size.z));
    }
}
