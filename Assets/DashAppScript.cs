﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashAppScript : MonoBehaviour
{
    //list of restaurants
    public GameObject HannahsWaypointBox;
    public GameObject FuneWaypointBox;

    public GameObject[] ListOfRestaurants;

    //list of Apartments
    public GameObject SeasandWaypointBox;
    public GameObject ChipsWaypointBox;

    public GameObject[] ListOfCustomers;

    //current app settings
    public GameObject CurrentRestaurant;
    public GameObject CurrentCustomer;

    public string CurrentOrderMessage;

    public Customer currentCustomerScript;

    public string DashAppStatus;

    public Canvas DisplayMessageCanvas;
    public Text DisplayMessageTextBox;

    public SFXScript sfxScript;

    private IEnumerator waitForOrderIEnumerator;

    // Start is called before the first frame update
    void Start()
    {
        ListOfCustomers = GameObject.FindGameObjectsWithTag("Customer");
        ListOfRestaurants = GameObject.FindGameObjectsWithTag("Restaurant");

        sfxScript = GameObject.Find("Main Camera").GetComponent<SFXScript>();

        StartANewOrder();
    }

    private IEnumerator WaitToOrder(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    public IEnumerator DefineANewOrder()
    {
        var randomAmountOfTimeToWait = Random.Range(2.0f, 4.0f);
        yield return new WaitForSeconds(randomAmountOfTimeToWait);
        pickARandomRestaurant();
        pickARandomCustomer();
        setDeliveryMessages();
        activateRestaurantWaypointBox();
        sfxScript.orderAlertSFX.Play();
        DashAppStatus = "waiting for pickup";
    }

    public void StartANewOrder()
    {
        DisplayMessageTextBox.text = "Waiting for an order.";
        var randomAmountOfTimeToWait = Random.Range(10.0f, 14.0f);
        StartCoroutine(WaitToOrder(randomAmountOfTimeToWait));
        StartCoroutine(DefineANewOrder());
    }

    void pickARandomRestaurant()
    {
        var RandomIndex = Random.Range(0, ListOfRestaurants.Length);
        CurrentRestaurant = ListOfRestaurants[RandomIndex];
    }

    void pickARandomCustomer()
    {
        var RandomIndex = Random.Range(0, ListOfCustomers.Length);
        CurrentCustomer = ListOfCustomers[RandomIndex];
        currentCustomerScript = CurrentCustomer.GetComponent<Customer>();
    }

    void setDeliveryMessages()
    {        
        if (CurrentRestaurant.name == "Hannahs")
        {
            CurrentOrderMessage = CurrentCustomer.name + " wants " + currentCustomerScript.orderFromHannahs + " from " + CurrentRestaurant.name + ". Drop off at " + currentCustomerScript.home.name;
        }
        else if (CurrentRestaurant.name == "Fune Sushi")
        {
            CurrentOrderMessage = CurrentCustomer.name + " wants " + currentCustomerScript.orderFromFune + " from " + CurrentRestaurant.name + ". Drop off at " + currentCustomerScript.home.name;
        }
        DisplayMessageTextBox.text = CurrentOrderMessage;
    }

    void activateRestaurantWaypointBox()
    {
        CurrentRestaurant.transform.Find("Waypoint Box").gameObject.SetActive(true);
    }

    void deactivateRestaurantWaypointBox()
    {
        CurrentRestaurant.transform.Find("Waypoint Box").gameObject.SetActive(false);
    }

    void activateCustomersHomeWaypointBox()
    {
        currentCustomerScript.home.transform.Find("Waypoint Box").gameObject.SetActive(true);
    }

    void deactivateCustomersHomeWaypointBox()
    {
        currentCustomerScript.home.transform.Find("Waypoint Box").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
