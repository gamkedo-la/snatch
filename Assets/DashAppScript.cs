﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashAppScript : MonoBehaviour
{
    public GameObject WayPointBoxRestaurant;
    public GameObject WayPointCustomer;

    public Transform[] restaurantSelection;
    public Transform[] customerLocations;

    public int restaurantSelected;
    public int customerLocation;
    public int customerName;

    public float OrderDuration = 3f;
    public float startingOrderDuration; //used for determining score
    bool orderSelected;

    public Text orderUI;
    public Text countDownTimer;

    public void Update()
    {
        OrderDuration -= Time.deltaTime;

        if (OrderDuration <= 0)
        {
            orderSelected = true;
        }

        if (GameManager.Player1OrderSelected)
        {
            WayPointBoxRestaurant.transform.position = restaurantSelection[restaurantSelected].transform.position;
            WayPointCustomer.transform.position = customerLocations[restaurantSelected].transform.position;

            GameManager.Player1OrderSelected = false;

            OrderDuration = 100; //remove this later, just for testing
            // startingOrderDuration = distance between restaurant selected and customer selected (this does not change)
            // OrderDuration = distance between restaurant selected and customer selected

            //This is the UI to tell you the order -> Deliver "Product" from "Restaurant" to "CustomerName" at "CustomerLocation"

            orderUI.text = "Deliver " + GameManager.Player1CustomerItemOrdered + " from " + $"<color=green>{GameManager.Player1RestaurantName}</color>" + " to " + GameManager.Player1CustomerName + " at " + $"<color=yellow>{GameManager.Player1ApartmentName}</color>";
        }

        //UI
        if (OrderDuration > 0)
        {
            countDownTimer.text = OrderDuration.ToString("F2");
        }

        else
        {
            countDownTimer.text = "";
        }
    }
}
