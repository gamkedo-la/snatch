﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabOrder : MonoBehaviour
{
    public string[] restaurantName;
    public string[] apartmentName;
    public string[] customerNames;
    public string[] orderedItems;

    Transform[] restaurantLocations;
    Transform[] apartmentLocations;

    public int restaurantSelected;
    public int customerLocation;
    public int customerName;

    public Text orderText;
    public Text orderState;

    public bool orderHasBeenTaken;
    public Image orderCondition;

    public GameObject accept, decline;
    GameObject pointer;

    GameObject player1RestaurantWayPoint, player1ApartmentWayPoint;

    private void Start()
    {
        restaurantLocations = new Transform[3];

        restaurantLocations[0] = GameObject.Find("HannahsWayPoint").transform;
        restaurantLocations[1] = GameObject.Find("RamenWayPoint").transform;
        restaurantLocations[2] = GameObject.Find("SushiWayPoint").transform;

        apartmentLocations = new Transform[4];

        apartmentLocations[0] = GameObject.Find("ChipsWayPoint").transform;
        apartmentLocations[1] = GameObject.Find("SeasandWayPoint").transform;
        apartmentLocations[2] = GameObject.Find("28 Apartment Waypoint").transform;
        apartmentLocations[3] = GameObject.Find("Halina WayPoint").transform;

        pointer = GameObject.Find("Pointer");

        player1RestaurantWayPoint = GameObject.Find("WayPointBox - Restaurant");
        player1ApartmentWayPoint = GameObject.Find("WayPointBox - Customer");

        restaurantSelected = Random.Range(0, restaurantName.Length);
        customerName = Random.Range(0, customerNames.Length);
        customerLocation = Random.Range(0, apartmentName.Length);

        orderText.text = customerNames[customerName].ToString() + " ordered " + orderedItems[restaurantSelected] + " from " + restaurantName[restaurantSelected].ToString() + " to deliver to " + apartmentName[customerLocation].ToString();
    }

    public void OrderAccepted()
    {
        orderHasBeenTaken = true;
        accept.SetActive(false);
        decline.SetActive(true);
        orderCondition.color = Color.green;

        pointer.SetActive(true);

        GameManager.Player1OrderSelected = true;
        print(GameManager.Player1OrderSelected);

        GameManager.Player1CustomerItemOrdered = orderedItems[restaurantSelected].ToString();
        GameManager.Player1ApartmentName = apartmentName[customerLocation].ToString();
        GameManager.Player1CustomerName = customerNames[customerName].ToString();
        GameManager.Player1RestaurantName = restaurantName[restaurantSelected].ToString();

        player1RestaurantWayPoint.transform.position = restaurantLocations[restaurantSelected].transform.position;
        
        player1ApartmentWayPoint.transform.position = apartmentLocations[customerLocation].transform.position;

        GameManager.player1Distance = Vector3.Distance(player1RestaurantWayPoint.transform.position, player1ApartmentWayPoint.transform.position);
    }

    public void OrderDeclined()
    {
        if (orderHasBeenTaken)
        {
            orderHasBeenTaken = false;
            accept.SetActive(true);
            decline.SetActive(false);
            orderCondition.color = Color.yellow;

            pointer.SetActive(false);
        }
    }
}
