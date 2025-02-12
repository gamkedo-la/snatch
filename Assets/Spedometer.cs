﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spedometer : MonoBehaviour
{
    public Text speed;
    public Image SpedometerFill;
    public ScooterDrive scooterDriveScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speedDisplay = scooterDriveScript.playerCurrentSpeed * 20;

        if (scooterDriveScript.playerCurrentSpeed == ScooterDrive.maxSpeed)
        {
            speedDisplay = (scooterDriveScript.playerCurrentSpeed * 20) + Random.Range(-.5f, .5f);
        }
        speed.text = speedDisplay.ToString("F2") + " km/h";

        SpedometerFill.fillAmount = speedDisplay / (ScooterDrive.maxSpeed * 20 * 1.4f);
    }
}
