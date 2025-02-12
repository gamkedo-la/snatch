﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    Transform[] Restaurants;
    Transform[] Apartments;

    Transform apartmentToGoTo;
    Transform restaurantToGoTo;

    bool orderSelected;
    bool orderPickedUp;
    bool orderDelivered;

    public NavMeshAgent agent;
    float dist;

    public GameObject target;

    Rigidbody rb;
    Vector3 previousPosition;
    float curSpeed;

    public GameObject toGoBox;

    public float orderScore;

    public bool enemy2, enemy3, enemy4;

    public int enemyScore;

    private DeliveryDriver myDriver;

    bool orderCompleted;

    void Start() {
        myDriver = GetComponent<DeliveryDriver>();

        #region Testing Score
        if (enemy2)
        {
            GameManager.enemy2ScoreTotal = Random.Range(0, 100);
            enemyScore = GameManager.enemy2ScoreTotal;
        }
        if (enemy3)
        {
            GameManager.enemy3ScoreTotal = Random.Range(0, 100);
            enemyScore = GameManager.enemy3ScoreTotal;
        }

        if (enemy4)
        {
            GameManager.enemy4ScoreTotal = Random.Range(0, 100);
            enemyScore = GameManager.enemy4ScoreTotal;
        }
        #endregion
        rb = GetComponent<Rigidbody>();

        #region Setting locations
        Restaurants = new Transform[9];

        Restaurants[0] = GameObject.Find("HannahsWayPoint").transform;
        // restaurantLocations[0].position = new Vector3(1450f, 2.1f, 250.9f);

        Restaurants[1] = GameObject.Find("RamenWayPoint").transform;

        // restaurantLocations[1].position = new Vector3(1450f, 2.1f, 250.9f);

        Restaurants[2] = GameObject.Find("SushiWayPoint").transform;

        // restaurantLocations[2].position = new Vector3(1450f, 2.1f, 250.9f);

        Restaurants[3] = GameObject.Find("PinkBrotherWaypoint").transform;
        Restaurants[4] = GameObject.Find("PunaWaypoint").transform;
        Restaurants[5] = GameObject.Find("AamerBakeryWaypoint").transform;
        Restaurants[6] = GameObject.Find("MudBistroWaypoint").transform;
        Restaurants[7] = GameObject.Find("BrosWaypoint").transform;
        Restaurants[8] = GameObject.Find("BikiniBottomWaypoint").transform;



        Apartments = new Transform[5];

        Apartments[0] = GameObject.Find("ChipsWayPoint").transform;
        Apartments[1] = GameObject.Find("SeasandWayPoint").transform;
        Apartments[2] = GameObject.Find("RioWaypoint").transform;
        Apartments[3] = GameObject.Find("Da Nang Beach Apartment Waypoint").transform;
        Apartments[4] = GameObject.Find("Vacation Beach Hotel Waypoint").transform;

        #endregion
        ChooseAnOrder();
    }

    void ChooseAnOrder()
    {
        orderPickedUp = false;
        orderSelected = false;
        toGoBox.SetActive(false);

        int Rselection = Random.Range(0, Restaurants.Length);
        int Aselection = Random.Range(0, Apartments.Length);

        restaurantToGoTo = Restaurants[Rselection];
        apartmentToGoTo = Apartments[Aselection];
      //  Debug.Log("restaurantToGoTo:" + restaurantToGoTo + "     " +  "apartmentToGoTo: " + apartmentToGoTo);

        orderScore = 100; 
        orderSelected = true;

        target.transform.position = restaurantToGoTo.transform.position;
    }


    void Update()
    {
        if (orderSelected)
        {
            orderScore -= Time.deltaTime;
        }

        if (orderSelected)
        {
            if (!orderPickedUp)
            {
                TravelToRestaurant();
            }
            if (orderPickedUp)
            {
                TravelToApartment();
            }
        }

        if(orderScore<= 0)
        {
            // cancel the order if you have taken too long
            toGoBox.SetActive(false);
            StartCoroutine(Waiting());
        }

        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;
    }

    void TravelToRestaurant() {
        agent.destination = target.transform.position;
        dist = Vector3.Distance(agent.transform.position, target.transform.position);
    }

    void TravelToApartment()
    {
        agent.destination = target.transform.position;
        dist = Vector3.Distance(agent.transform.position, target.transform.position);
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(3);
        curSpeed = 0;
        target.GetComponent<BoxCollider>().enabled = true;
        ChooseAnOrder();
    }

    IEnumerator Waiting2()
    {
        yield return new WaitForSeconds(1);
        curSpeed = 0;
        toGoBox.SetActive(true);
        orderPickedUp = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AITarget")
        {
            if (orderPickedUp)
            {
                target.GetComponent<BoxCollider>().enabled = false;
                //print("Order Delivered");
                if (!orderCompleted)
                {
                    myDriver.IncreaseOrderTotal((int)orderScore);
                    orderScore = 0;
                    orderCompleted = true;
                }
                StartCoroutine(Waiting());
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "AITarget")
        {
            if (curSpeed <= 1f)
            {
                if (!orderPickedUp)
                {
                    //print("Order PickedUp");
                    //This line is a problem child, I'll (Cass) look into it more
                    //print("This is the target (restaurant to go to)  " + target.name);
                    //print("This is the apt to go to   " + apartmentToGoTo.name);
                    target.transform.position = apartmentToGoTo.transform.position;
                    //
                    orderCompleted = false;
                    StartCoroutine(Waiting2());
                    orderPickedUp = true;
                }                
            }
        }
    }
}
