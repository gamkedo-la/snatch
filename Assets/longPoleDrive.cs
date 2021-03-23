﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class longPoleDrive : MonoBehaviour
{
    public float longPoleSpeed = 0.44f;
    // Update is called once per frame
    void Update()
    {
        Vector3 distanceToTranslate = new Vector3(0, 0, -longPoleSpeed);
        transform.Translate(distanceToTranslate);
    }
}
