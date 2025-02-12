﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsOnPlayer : MonoBehaviour
{
    Rigidbody rb;
    float RandX, RandY, RandZ;

    float RandForceZ;
    float RandForceX;

    bool hasBeenKnockedOver;

    public float lifetimeAfterKnockedOver = 10;
    public Material transparent;
    public MeshRenderer DestObject;
    float transformMaterial;

	public AudioClip[] hitSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transformMaterial = lifetimeAfterKnockedOver * .2f;
        //print(transformMaterial);
    }

    private void Update()
    {
        if (hasBeenKnockedOver)
        {
            lifetimeAfterKnockedOver -= Time.deltaTime;
        }

        if (lifetimeAfterKnockedOver <= transformMaterial)
        {
            DestObject.material = transparent;
        }

        if (lifetimeAfterKnockedOver <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var scooterDriveScript = other.transform.GetComponent<ScooterDrive>();

            RandX = Random.Range(-360, 360);
            RandY = Random.Range(270, 360);
            RandZ = Random.Range(-360, 360);

            if (ScooterDrive.isMovingNorth)
            {
                RandForceZ = Random.Range(-scooterDriveScript.playerCurrentSpeed / 8,-scooterDriveScript.playerCurrentSpeed / 4);
            }
            else if (ScooterDrive.isMovingSouth)
            {
                RandForceZ = Random.Range(scooterDriveScript.playerCurrentSpeed / 4 , scooterDriveScript.playerCurrentSpeed / 8);
            }

            if (ScooterDrive.isMovingEast)
            {
                RandForceX = Random.Range(-scooterDriveScript.playerCurrentSpeed / 8,-scooterDriveScript.playerCurrentSpeed / 4);
            }
            else if (ScooterDrive.isMovingWest)
            {
                RandForceX = Random.Range(scooterDriveScript.playerCurrentSpeed / 4 , scooterDriveScript.playerCurrentSpeed / 8);
            }

            GetComponent<Rigidbody>().AddForce(new Vector3(RandForceX, scooterDriveScript.playerCurrentSpeed, RandForceZ), ForceMode.Impulse);
            this.transform.rotation = Quaternion.Euler(RandX, RandY, RandZ);
            hasBeenKnockedOver = true;
        }

        if (hitSound != null && hitSound.Length > 0)
        {
			AudioManager.Instance.PlaySoundSFX(hitSound[Random.Range(0, hitSound.Length)], gameObject);
        }
    }
}
