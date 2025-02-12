﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabOrderPlayer3 : MonoBehaviour
{
    public string[] restaurantName;
    public string[] apartmentName;
    public string[] customerNames;
    public string[] orderedItems;

    Transform[] restaurantLocations;
    Transform[] apartmentLocations;

    Transform playerLocation;

    public int restaurantSelected;
    public int customerLocation;
    public int customerName;

    public Text orderText;
    public Text orderState;

    public static bool orderHasBeenTaken3;
    public Image orderCondition;

    public GameObject accept, decline, displayOrdersText, gameManager;

    GameObject player2WayPoint, pointer, pointerCube;
    Transform Player2RestaurantTransform, Player2ApartmentTransform;

    PointToObjective pointToObjectiveScript;
    private GraphPathfinding gpsScript;
    private GraphNode restaurantNode;

    private AudioClip phoneButtonPressedAudioClip;
    private PhoneScript phoneScript;

    public float minOrderTime = 10;
    public float maxOrderTime = 10;

    PopulateGrid populateGrid;

    public bool isFocusedOn = false;

    private void Start()
    {
        populateGrid = GameObject.Find("Content3").GetComponent<PopulateGrid>();
        phoneScript = GameObject.Find("Canvas - PlayerPhone3").GetComponent<PhoneScript>();
        phoneButtonPressedAudioClip = phoneScript.phoneButtonPressedAudioClip;

        gameManager = GameObject.Find("GameManager");
        //Debug.Log(gameManager);
        GameObject masterKeyWaypoint = GameObject.Find("Player1Restaurant");

        if (masterKeyWaypoint == null)
        {
            Debug.Log("masterKeyWaypoint not found, bailing on prefab orders");
            this.enabled = false;
            return;
        }

        Player2RestaurantTransform = GameObject.Find("Player3Restaurant").transform;
        Player2ApartmentTransform = GameObject.Find("Player3Apartment").transform;

        restaurantLocations = new Transform[9];

        restaurantLocations[0] = GameObject.Find("HannahsWayPoint").transform;
        restaurantLocations[1] = GameObject.Find("RamenWayPoint").transform;
        restaurantLocations[2] = GameObject.Find("SushiWayPoint").transform;
        restaurantLocations[3] = GameObject.Find("PinkBrotherWaypoint").transform;
        restaurantLocations[4] = GameObject.Find("PunaWaypoint").transform;
        restaurantLocations[5] = GameObject.Find("AamerBakeryWaypoint").transform;
        restaurantLocations[6] = GameObject.Find("MudBistroWaypoint").transform;
        restaurantLocations[7] = GameObject.Find("BrosWaypoint").transform;
        restaurantLocations[8] = GameObject.Find("BikiniBottomWaypoint").transform;



        apartmentLocations = new Transform[5];

        apartmentLocations[0] = GameObject.Find("ChipsWayPoint").transform;
        apartmentLocations[1] = GameObject.Find("SeasandWayPoint").transform;
        apartmentLocations[2] = GameObject.Find("RioWaypoint").transform;
        apartmentLocations[3] = GameObject.Find("Da Nang Beach Apartment Waypoint").transform;
        apartmentLocations[4] = GameObject.Find("Vacation Beach Hotel Waypoint").transform;

        // apartmentLocations[0].position = new Vector3(1550f, 2.1f, 250.9f);
        // apartmentLocations[1].position = new Vector3(1550f, 2.1f, 250.9f);
        //  apartmentLocations[2] = GameObject.Find("28 Apartment Waypoint").transform;
        //  apartmentLocations[3] = GameObject.Find("Halina WayPoint").transform;

        player2WayPoint = GameObject.Find("WayPointBox - Restaurant3");
        // player1ApartmentWayPoint = GameObject.Find("WayPointBox - Customer");

        restaurantSelected = Random.Range(0, restaurantName.Length);
        customerName = Random.Range(0, customerNames.Length);
        customerLocation = Random.Range(0, apartmentName.Length);

        gpsScript = FindObjectOfType<GraphPathfinding>();

        orderText.text = customerNames[customerName].ToString() + " ordered " + orderedItems[restaurantSelected] + " from " + restaurantName[restaurantSelected].ToString() + " to deliver to " + apartmentName[customerLocation].ToString();
    }

    private void Update()
    {
        if (isFocusedOn)
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color32(11, 63, 156, 125);
            //gameObject.GetComponent<Image>().color = new Color32(11, 63, 156, 125);
        }
        else
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 196, 0, 255);
            //gameObject.GetComponent<Image>().color = new Color32(255, 196, 0, 255);
        }
    }
    public void OrderAccepted()
    {
        if (!orderHasBeenTaken3)
        {
            AudioManager.Instance.PlaySoundSFX(phoneButtonPressedAudioClip, gameObject, volume: 0.5f);

            orderHasBeenTaken3 = true;
            orderCondition.color = Color.green;

            if (MainMenu.normal)
            {
                pointer = GameObject.Find("Canvas - Display Orders").transform.GetChild(2).gameObject;
                pointer.SetActive(true);
                pointToObjectiveScript = pointer.GetComponent<PointToObjective>();

                pointerCube = pointer.transform.GetChild(2).gameObject;
                pointerCube.SetActive(false);
            }
            //    pointToObjectiveScript.customerOrder = gameObject;
            //Debug.Log("restaurant name: " + restaurantName);



            GameManager.Player3OrderSelected = true;
            //print(GameManager.Player1OrderSelected);

            GameManager.Player3CustomerItemOrdered = orderedItems[restaurantSelected].ToString();
            GameManager.Player3ApartmentName = apartmentName[customerLocation].ToString();
            GameManager.Player3CustomerName = customerNames[customerName].ToString();
            GameManager.Player3RestaurantName = restaurantName[restaurantSelected].ToString();

            Player2RestaurantTransform.transform.position = restaurantLocations[restaurantSelected].transform.position;
            Player2ApartmentTransform.transform.position = apartmentLocations[customerLocation].transform.position;

            float dist = Vector3.Distance(Player2RestaurantTransform.position, Player2ApartmentTransform.position) / 5;

            if (dist < minOrderTime)
            {
                RestaurantWaypoint3.player3TimeScore = minOrderTime;
                RestaurantWaypoint3.player3TimeScoreMax = minOrderTime;
            }
            else if (dist > maxOrderTime)
            {
                RestaurantWaypoint3.player3TimeScore = maxOrderTime;
                RestaurantWaypoint3.player3TimeScoreMax = maxOrderTime;
            }
            else
            {
                RestaurantWaypoint3.player3TimeScore = 125f;
                RestaurantWaypoint3.player3TimeScoreMax = 125f;
            }

            player2WayPoint.transform.position = Player2RestaurantTransform.transform.position;

            RestaurantWaypoint3.player3OrderPickedUp = false;
            RestaurantWaypoint3.player3OrderDelivered = false;

            playerLocation = FindObjectOfType<ScooterDrive>().transform;
            restaurantNode = restaurantLocations[restaurantSelected].GetComponent<GraphNode>();
            gpsScript.FindPath(gpsScript.FindNearestNode(playerLocation), restaurantNode);

            StartCoroutine(Waiting());
        }    
        
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("DeductCount player3 reached");
        //populateGrid.DeductCount();
        //Destroy(this.gameObject);
    }
}
