﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StebsNPCLizInABagDrive : MonoBehaviour
{
    public float lizInABagSpeed = 0.44f;
    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToTranslate = new Vector3(0, 0, lizInABagSpeed);
        transform.Translate(distanceToTranslate);
    }
}