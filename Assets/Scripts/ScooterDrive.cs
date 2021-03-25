﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class ScooterDrive : MonoBehaviour
{

    PlayerControls controls;

    public bool player1, player2, player3, player4;

    public GameObject[] playerCharacter;

    //this variable is used to determine how much force to apply to enviornment objects after collision
    public float playerCurrentSpeed;
    public static float maxSpeed = 15f;

    public float currentSpeed;
    public float forwardSpeed = 0.001f;
    public float backwardSpeed = -10.1f;
    public bool backingUp = false;
    public float coastToStopSpeed = 0.25f;

    public float currentTurnAngle = 0;
    public float turnAngleRate = 200f;
    public float maxTurnAngle = 1000f;

    public float maxTiltAngle = 45f;
    public float currentBikeTiltAngle = 0.0f;
    bool maxRightTurnHeld = false;
    bool maxLeftTurnHeld = false;

    public float bikeStraightenSpeed = 60f;
    public float bikeMaxTiltAngle = 0.02f;
    public float bikeCrashWithAICarBumbForce = 5.0f;

    public float brakeSpeed = 0;
    public float brakeMaxSpeed = 15f;
    private float brakeAccelerating = 0.025f; //Change this if you want to tweak braking.

    public GameObject brakeLights;

    public Transform restartAt;
    Rigidbody scootersRigidbodyComponent;

    public GameObject bikeModel;

    public Animator phone;

    static bool phoneToggle;

    public static bool isMovingNorth = false;
    public static bool isMovingEast = false;
    public static bool isMovingSouth = false;
    public static bool isMovingWest = false;

    public float previousX;// east/west
    public float previousZ;// north/south

    public GameObject collisionParticle;
    public float speedPenaltyPercent = 15;

    public GameObject physicalOrder;

    public GameObject TotalScore, star1, star2, star3, star4, star5;
    public double star2threshold = 20;
    public double star3threshold = 50;
    public double star4threshold = 70;
    public double star5threshold = 85;
    public RatingsManager ratingsManager;
    public AudioClip bikeIdleAudioClip, bikeAccelleratingAudioClip, bikeLetOffGasAudioClip, bikeTopSpeedClip, phoneInOutAudioClip;
    private AudioSource bikeCurrentAudioSource;

	public GameObject textTip;

    bool canBeCollided;
    public float collisionPercent;
    public GameObject bike, playerModel;

    //Input Controls
    public int playerIndex = 0;

    public bool isAccelerating = false;
    public bool acceleratingCompleted = false;
    public bool turnLeft = false;
    public bool turnRight = false;
    public bool isBraking = false;
    public bool isBrakingCompleted = false;
    public bool isReversing = false;
    public bool isReversingCompleted = false;
    public bool isAbleToReverse = false;
    public int accelerateValue;

    private HomeScreenScript homeScreenScript;
    public bool phoneActive = false;

    DeliveryDriver driverID;

    public GameObject phoneCanvas;

    public GameObject phoneGameObject;

    public GameObject homeScreen;
    public GameObject ordersScreen;
    public GameObject individualOrdersHolder;
    public int focusedOrderIndex = 0;

    float tempScore;

    private void Awake()
    {
        //homeScreen = GameObject.Find("HomeScreen");
        

        //controls = new PlayerControls();

        //controls.GamePlay.Accelerate.performed += context => { isAccelerating = true; };
        //controls.GamePlay.Accelerate.canceled += context => { isAccelerating = false; acceleratingCompleted = true; };
        ////controls.GamePlay.AccelerateStick.performed += context =>
        ////{
        ////        isAccelerating = true; 
        ////        isAcceleratingFromStick = true;
        ////        acceleratingCompleted = false;
        ////};
        ////controls.GamePlay.AccelerateStick.canceled += context => { isAccelerating = false; acceleratingCompleted = true; isAcceleratingFromStick = false; };
        //controls.GamePlay.TurnLeft.performed += context => { /*Debug.Log("left stick recognized");*/ turnLeft = true;  } ;
        //controls.GamePlay.TurnLeft.canceled += context => { /*Debug.Log("left stick recognized");*/ turnLeft = false; };
        //controls.GamePlay.TurnRight.performed += context => { /*Debug.Log("left stick recognized");*/ turnRight = true; };
        //controls.GamePlay.TurnRight.canceled += context => { /*Debug.Log("left stick recognized");*/ turnRight = false; };
        //controls.GamePlay.Brake.performed += context => { /*Debug.Log("right trigger is recognized");*/ isBraking = true; };
        //controls.GamePlay.Brake.canceled += context => { isBraking = false; isBrakingCompleted = true; };
        //controls.GamePlay.PhoneOutIn.performed += context => { PhoneOutIn(); };

        //controls.GamePlay.navigateUIUp.performed += context => {
        //    HandleNavigateUIUp();
        //};

        //controls.GamePlay.navigateUIRight.performed += context => {
        //    HandleNavigateUIRight();
        //};

        //controls.GamePlay.navigateUIDown.performed += context => {
        //    HandleNavigateUIDown();
        //};

        //controls.GamePlay.navigateUILeft.performed += context => {
        //    HandleNavigateUILeft();
        //};
    }

    private void OnEnable()
    {
        //controls.GamePlay.Enable();
    }

    private void OnDisable()
    {
        //controls.GamePlay.Disable();
    }

    void Start()
    {
        phone.SetBool("PhoneOn", false);
        //RestartAtSpawn();
        driverID = GetComponent<DeliveryDriver>();

        scootersRigidbodyComponent = gameObject.GetComponent<Rigidbody>();
        if (scootersRigidbodyComponent == null)
        {
            Debug.Log("Scooter setup incorrectly, no rigidbody found");
        }

        homeScreenScript = phone.GetComponentInChildren<HomeScreenScript>();


        bikeCurrentAudioSource = AudioManager.Instance.PlaySoundSFX(bikeIdleAudioClip, gameObject, loop: true, volume: 0.1f);

        if (player1)
        {
            playerCharacter[MainMenu.player1Character].SetActive(true);
        }
        if (player2)
        {
            playerCharacter[MainMenu.player2Character].SetActive(true);
        }
        if (player3)
        {
            playerCharacter[MainMenu.player3Character].SetActive(true);
        }
        if (player4)
        {
            playerCharacter[MainMenu.player4Character].SetActive(true);
        }
    }

    //This is the area where you toggle the phone on and off

    //public void PhoneOutIn()
    //{
        
        
    //    PhoneActivation();
    //}

    //void PhoneActivation()
    //{
        

        
    //}

    

    // Update is called once per frame
    void Update()
    {
        if (CountdownImagesController.canDrive && !GameManager.roundOver)
        {
            HandleControlKeys();
            MoveBikeForwardOrBackward();
            TurnBikeLeftOrRight();
            if (currentTurnAngle != 0)
            {

            }
            updateDirectionBools();

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (player1)
                {
                    if (phone.GetBool("PhoneOn"))
                    {
                        return;
                    }
                    //TakePhoneOutOfPocket();
                }
            }

            playerCurrentSpeed = currentSpeed;

            previousX = gameObject.transform.position.x;
            previousZ = gameObject.transform.position.z;

            if (currentSpeed >= ((collisionPercent / 100) * maxSpeed))
            {
                canBeCollided = true;
            }

            else
            {
                canBeCollided = false;
            }
        }
    }// end of update(){}

    public void updateDirectionBools()
    {
        if (gameObject.transform.position.z > previousZ)
        {
            isMovingNorth = true;
            isMovingSouth = false;
        }
        else 
        {
            isMovingSouth = true;
            isMovingNorth = false;
        }

        if (gameObject.transform.position.x > previousX)
        {
            isMovingWest = false;
            isMovingEast = true;
        }
        else
        {
            isMovingWest = true;
            isMovingEast = false;
        }
    }

    public void RestartAtSpawn()
    {
        transform.position = restartAt.position;
        transform.rotation = restartAt.rotation;

        stopScooterMovement();
    }

    void stopScooterMovement()
    {
        scootersRigidbodyComponent.angularVelocity = Vector3.zero;
        currentSpeed = 0.0f;
        currentTurnAngle = 0.0f;
    }

    void MoveBikeForwardOrBackward()
    {
        transform.position += transform.forward * currentSpeed * 3;
    }

    void TurnBikeLeftOrRight()
    {
        if (currentTurnAngle != 0)
        {
            transform.Rotate(Vector3.up, currentTurnAngle);
            
        }
    }

    /*void Accelerate()
    {
        currentSpeed += forwardSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
            if (bikeCurrentAudioSource.clip != bikeTopSpeedClip) {
				AudioManager.Instance.StopSound(bikeCurrentAudioSource);
				bikeCurrentAudioSource = AudioManager.Instance.PlaySoundSFX(bikeTopSpeedClip, gameObject, loop: true, volume: 0.25f);
            }
        }
		else if (currentSpeed < maxSpeed && bikeCurrentAudioSource.clip != bikeAccelleratingAudioClip) {
			AudioManager.Instance.StopSound(bikeCurrentAudioSource);
			bikeCurrentAudioSource = AudioManager.Instance.PlaySoundSFX(bikeAccelleratingAudioClip, gameObject, loop: true, volume: 0.25f);
		}
    }*/

    void HandleControlKeys()
    {
        if (isAccelerating && (isReversing || isBraking))
        {
            accelerateValue = 1;
        }
        else if (isAccelerating)
        {
            //accelerateValue = 1;
        }
        else if (!isAccelerating && (isReversing || isBraking))
        {
            accelerateValue = -1;
        }
        else
        {
            accelerateValue = 0;
        }

        if (isBraking)
        {
            brakeSpeed += brakeAccelerating * Time.deltaTime;

            if (brakeSpeed > brakeMaxSpeed)
            {
                brakeSpeed = brakeMaxSpeed;
            }
        }
        else
        {
            brakeSpeed = 0.0f;
        }

        //if (isBraking)
        //{
        //    isAbleToReverse = currentSpeed > 0 ? false : true;
        //}

        //if (isAbleToReverse)
        //{
        //    isReversing = true;
        //    isReversingCompleted = false;
        //}
        //else if (!isBraking && isReversing )
        //{
        //    isReversing = false;
        //    isReversingCompleted = true;
        //}

        //forward and backwards
        if (isAccelerating)
        {

            currentSpeed += /*forwardSpeed */ 0.3f * Time.deltaTime /* * (accelerateValue) */; 
            //Debug.Log("current speed: " + currentSpeed);
            if (currentSpeed > 0.7f)
            {
                currentSpeed = 0.7f;
                //** BELOW TO BE USED WHEN WE HAVE A BETTER TOP SPEED SOUND
				/*if (bikeCurrentAudioSource.clip != bikeTopSpeedClip) 
				{
					AudioManager.Instance.StopSound(bikeCurrentAudioSource);
					bikeCurrentAudioSource = AudioManager.Instance.PlaySoundSFX(bikeTopSpeedClip, gameObject, loop: true, volume: 0.25f);
				}*/
			}
			else if (currentSpeed < 1.25f && bikeCurrentAudioSource.clip != bikeAccelleratingAudioClip) 
			{
				AudioManager.Instance.StopSound(bikeCurrentAudioSource);
				bikeCurrentAudioSource = AudioManager.Instance.PlaySoundSFX(bikeAccelleratingAudioClip, gameObject, loop: true, volume: 0.25f);
			}

            //if (currentSpeed > maxSpeed)
            //{
             //   currentSpeed = maxSpeed;
            //}    
		}
        else if (!isAccelerating && currentSpeed > 0)
        {
            currentSpeed += Time.deltaTime * brakeSpeed * accelerateValue;
            if (currentSpeed < 0)
            {
                currentSpeed = 0;
            }
        }
        

        //let off gas sfx
        if (acceleratingCompleted)
        {
			AudioManager.Instance.StopSound(bikeCurrentAudioSource);
			bikeCurrentAudioSource = AudioManager.Instance.PlaySoundSFX(bikeIdleAudioClip, gameObject, loop: true, volume: 0.1f);
			AudioManager.Instance.PlaySoundSFX(bikeLetOffGasAudioClip, gameObject, volume: 0.25f);

			acceleratingCompleted = false;
        }

        //turns
        if (turnRight)
        {
            if (maxLeftTurnHeld || maxRightTurnHeld)
            {
                Debug.Log("maxTurnReached");
                //return;
            }
            else
            {
                //Debug.Log("bikeModel.transform.localRotation: " + bikeModel.transform.localRotation);

                currentBikeTiltAngle = bikeModel.transform.localRotation.z;
                if (currentBikeTiltAngle < (-maxTiltAngle))
                {
                    bikeModel.transform.localRotation = Quaternion.Euler(0, 0, currentBikeTiltAngle * 100);
                    maxRightTurnHeld = true;
                }

                if (bikeModel.transform.localRotation.z > 0)
                {
                    bikeModel.transform.Rotate(Vector3.forward * Time.deltaTime * -bikeStraightenSpeed);
                    currentTurnAngle = 0;
                }
                else
                {
                    bikeModel.transform.Rotate(-Vector3.forward * Time.deltaTime * 20);

                    //Debug.Log("currentTurnAngle: " + currentTurnAngle);
                    currentTurnAngle += Time.deltaTime * turnAngleRate;
                    if (currentTurnAngle > maxTurnAngle)
                    {
                        currentTurnAngle = maxTurnAngle;
                    }
                }
            }
        }
        
        if (turnLeft)
        {
            if (maxLeftTurnHeld || maxRightTurnHeld)
            {
                //return;
            }
            else
            {
                //Debug.Log("bikeModel.transform.localRotation: " + bikeModel.transform.localRotation);
                //Debug.Log("bikeTiltAngle: " + bikeModel.transform.localRotation.z);

                currentBikeTiltAngle = bikeModel.transform.localRotation.z;
                if (currentBikeTiltAngle > (maxTiltAngle))
                {
                    bikeModel.transform.localRotation = Quaternion.Euler(0, 0, currentBikeTiltAngle * 100);
                    maxLeftTurnHeld = true;
                }

                if (bikeModel.transform.localRotation.z < 0)
                {
                    bikeModel.transform.Rotate(Vector3.forward * Time.deltaTime * bikeStraightenSpeed);
                    currentTurnAngle = 0;
                }
                else
                {
                    bikeModel.transform.Rotate(Vector3.forward * Time.deltaTime * 20);
                    // Debug.Log("currentTurnAngle: " + currentTurnAngle);
                    currentTurnAngle += -Time.deltaTime * turnAngleRate;
                    if (currentTurnAngle < -maxTurnAngle)
                    {
                        currentTurnAngle = -maxTurnAngle;
                    }
                }
            }
        }
        if (!turnRight && !turnLeft)
        {
            maxRightTurnHeld = false;
            maxLeftTurnHeld = false;

            currentTurnAngle = 0;
            //Debug.Log("bikeModel.transform.localRotation: " + bikeModel.transform.localRotation);
            if (bikeModel)
            {
                if (bikeModel.transform.localRotation.z > 0)
                {
                    bikeModel.transform.Rotate(Vector3.forward * Time.deltaTime * -bikeStraightenSpeed);
                }
                else if (bikeModel.transform.localRotation.z < 0)
                {
                    bikeModel.transform.Rotate(Vector3.forward * Time.deltaTime * bikeStraightenSpeed);
                }

                //Debug.Log("currentTurnAngle: " + currentTurnAngle);
                if (bikeModel.transform.localRotation.z < bikeMaxTiltAngle && bikeModel.transform.localRotation.z > -bikeMaxTiltAngle)
                {
                    //Debug.Log("local rotation being manipulated");
                    // bikeModel.transform.localRotation = Quaternion.Euler(0, 0, 0.0f);
                    bikeModel.transform.localRotation = Quaternion.identity;
                }
            }
            

        }

        if (currentSpeed == 0 && bikeCurrentAudioSource.clip != bikeIdleAudioClip) {
			AudioManager.Instance.StopSound(bikeCurrentAudioSource);
			bikeCurrentAudioSource = AudioManager.Instance.PlaySoundSFX(bikeIdleAudioClip, gameObject, loop: true, volume: 0.1f);
		}

		//brake
		if (isBraking && isAccelerating)
        {
            //brakeSpeed is currently 0.025f * Time.deltaTime 
            currentSpeed += brakeSpeed * currentSpeed;
            if (currentSpeed < 0)
            {
                currentSpeed = 0;
            }
            if (currentSpeed > 0.7f)
            {
                currentSpeed = 0.7f;
                //** BELOW TO BE USED WHEN WE HAVE A BETTER TOP SPEED SOUND
                /*if (bikeCurrentAudioSource.clip != bikeTopSpeedClip) 
				{
					AudioManager.Instance.StopSound(bikeCurrentAudioSource);
					bikeCurrentAudioSource = AudioManager.Instance.PlaySoundSFX(bikeTopSpeedClip, gameObject, loop: true, volume: 0.25f);
				}*/
            }
        }
        else if (isBraking && !isAccelerating)
        {
            currentSpeed += brakeSpeed * accelerateValue * 3;
            if (currentSpeed < 0)
            {
                //Debug.Log("inside current speed less than 0 check");
                currentSpeed = 0;
                isBrakingCompleted = true;
            }

            brakeLights.SetActive(true);
        }


        if (isReversing && currentSpeed == 0 && !isAccelerating)
        {
            backingUp = true;
        }
        if (isReversingCompleted)
        {
            backingUp = false;
        }

        if (backingUp)
        {
            //Debug.Log("inside backing up check");
            currentSpeed = backwardSpeed;
            brakeLights.SetActive(true);
            return;
        }

        if (isBraking || isReversing)
        {
            brakeLights.SetActive(true);
        }
        if (isBrakingCompleted || isReversingCompleted)
        {
            brakeLights.SetActive(false);
        }

        //coasting to stop
        if ((!isBraking && !isAccelerating && !isReversing))
        {
            currentSpeed -= Time.deltaTime * coastToStopSpeed;
            if (currentSpeed < 0)
            {
                currentSpeed = 0;
            }
        }

        //if (isAccelerating)
        //{
        //    accelerate();
        //}
    }

    

    void CheckIfPhoneIsActive()
    {
        Debug.Log(phone.GetBool("PhoneOn"));
    }

    public void AssignStars()
    {
        if (player1)
        {
            GameManager.player1OrderDelivered = true;
        }
        if (player2)
        {
            GameManager.player2OrderDelivered = true;
        }
        if (player3)
        {
            GameManager.player3OrderDelivered = true;
        }
        if (player4)
        {
            GameManager.player4OrderDelivered = true;
        }
        physicalOrder.SetActive(false);

        if (player1)
        {
            tempScore = (driverID.scoreOnOrder + FoodHealth.currentHealth1) / 2;
        }

        if (player2)
        {
            tempScore = (driverID.scoreOnOrder + FoodHealth.currentHealth2) / 2;
        }

        if (player3)
        {
            tempScore = (driverID.scoreOnOrder + FoodHealth.currentHealth3) / 2;
        }

        if (player4)
        {
            tempScore = (driverID.scoreOnOrder + FoodHealth.currentHealth4) / 2;
        }

/*
        int starsAwarded = 0;

        //total score added to macrototal score
        TotalScore.SetActive(true);
        if (tempScore < star2threshold && tempScore > 0)
        {
            star1.SetActive(true);
            starsAwarded++;
        }
        if (tempScore > star2threshold)
        {
            star2.SetActive(true);
            starsAwarded++;
        }
        if (tempScore > star3threshold)
        {
            star3.SetActive(true);
            starsAwarded++;
        }
        if (tempScore > star4threshold)
        {
            star4.SetActive(true);
            starsAwarded++;
        }
        if (tempScore > star5threshold)
        {
            star5.SetActive(true);
            starsAwarded++;
        }
       // print(tempScore + ":" + driverID.scoreOnOrder + "+" + FoodHealth.currentHealth);

        var rating = ratingsManager.CreateRating(starsAwarded);
        ratingsManager.AddRating(rating);
*/
        StartCoroutine(Waiting());
        //display score
        //turn off player1OrderPickedUp and player1OrderDelivered
        //turn off waypoint boxes
        //close app line for the order
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DestEnv")
        {
            Instantiate(collisionParticle, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), transform.rotation);
            playerCurrentSpeed -= speedPenaltyPercent;
            if (player1)
            {
                FoodHealth.currentHealth1 -= 5f;
            }
            if (player2)
            {
                FoodHealth.currentHealth2 -= 5f;
            }
            if (player3)
            {
                FoodHealth.currentHealth3 -= 5f;
            }
            if (player4)
            {
                FoodHealth.currentHealth4 -= 5f;
            }

            //print(playerCurrentSpeed);
        }

        if (other.tag == "WayPointBox")
        {
            if (player1)
            {
                GameManager.player1OrderPickedUp = true;
            }
            if (player2)
            {
                GameManager.player2OrderPickedUp = true;
            }
            if (player3)
            {
                GameManager.player3OrderPickedUp = true;
            }
            if (player4)
            {
                GameManager.player4OrderPickedUp = true;
            }
            physicalOrder.SetActive(true);
        }

        if (other.tag == "Building")
        {
            currentSpeed = 0;
            if (player1)
            {
                FoodHealth.currentHealth1 -= 5f;
            }
            if (player2)
            {
                FoodHealth.currentHealth2 -= 5f;
            }
            if (player3)
            {
                FoodHealth.currentHealth3 -= 5f;
            }
            if (player4)
            {
                FoodHealth.currentHealth4 -= 5f;
            }
        }

        if (other.tag == "Pedestrian")
        {
            currentSpeed = 0;
            if (player1)
            {
                FoodHealth.currentHealth1 -= 5f;
            }
            if (player2)
            {
                FoodHealth.currentHealth2 -= 5f;
            }
            if (player3)
            {
                FoodHealth.currentHealth3 -= 5f;
            }
            if (player4)
            {
                FoodHealth.currentHealth4 -= 5f;
            }
        }

        if (other.tag == "AICar")
        {
            if (player1)
            {
                FoodHealth.currentHealth1 -= 5f;
            }
            if (player2)
            {
                FoodHealth.currentHealth2 -= 5f;
            }
            if (player3)
            {
                FoodHealth.currentHealth3 -= 5f;
            }
            if (player4)
            {
                FoodHealth.currentHealth4 -= 5f;
            }
            currentSpeed = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "AICar")
        {
            currentSpeed = 0;
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(2);
        TotalScore.SetActive(false);
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
        star4.SetActive(false);
        star5.SetActive(false);
    }

    IEnumerator FallingOffBike()
    {
        yield return new WaitForSeconds(1);
        playerModel.transform.localPosition = new Vector3(0.1207f, 0f, .6239f);
        playerModel.transform.localRotation = Quaternion.identity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "building")
        {
            
            currentSpeed = 0;
        }
    }

    public void HandleNavigateUIUp()
    {
        //Debug.Log(gameObject.name);

        if (homeScreen.activeSelf)
        {
            homeScreenScript.dPadUpPressed = true;
            homeScreenScript.handleGamepadUINavigation();
        }
        else if (ordersScreen.activeSelf)
        {
            
            
            focusedOrderIndex--;
            if (focusedOrderIndex < 0)
            {
                focusedOrderIndex = 0;
            }
            if (gameObject.name == "Player1")
            {
                individualOrdersHolder.transform.GetChild(focusedOrderIndex).GetComponent<PrefabOrder>().isFocusedOn = true;
                individualOrdersHolder.transform.GetChild(focusedOrderIndex + 1).GetComponent<PrefabOrder>().isFocusedOn = false;
            }
            else if (gameObject.name == "Player2")
            {
                individualOrdersHolder.transform.GetChild(focusedOrderIndex).GetComponent<PrefabOrderPlayer2>().isFocusedOn = true;
                individualOrdersHolder.transform.GetChild(focusedOrderIndex + 1).GetComponent<PrefabOrderPlayer2>().isFocusedOn = false;
            }
            else if (gameObject.name == "Player3")
            {
                individualOrdersHolder.transform.GetChild(focusedOrderIndex).GetComponent<PrefabOrderPlayer3>().isFocusedOn = true;
                individualOrdersHolder.transform.GetChild(focusedOrderIndex + 1).GetComponent<PrefabOrderPlayer3>().isFocusedOn = false;
            }
            else if (gameObject.name == "Player4")
            {
                individualOrdersHolder.transform.GetChild(focusedOrderIndex).GetComponent<PrefabOrderPlayer4>().isFocusedOn = true;
                individualOrdersHolder.transform.GetChild(focusedOrderIndex + 1).GetComponent<PrefabOrderPlayer4>().isFocusedOn = false;
            }
        }
        
        //CheckIfPhoneIsActive();
    }

    public void HandleNavigateUIDown()
    {
        if (homeScreen.activeSelf)
        {
            homeScreenScript.dPadDownPressed = true;
            homeScreenScript.handleGamepadUINavigation();
        }
        else if (ordersScreen.activeSelf)
        {
            focusedOrderIndex++;
            if (focusedOrderIndex > individualOrdersHolder.transform.childCount - 1)
            {
                focusedOrderIndex = individualOrdersHolder.transform.childCount - 1;
            }

            if (gameObject.name == "Player1")
            {
                individualOrdersHolder.transform.GetChild(focusedOrderIndex).GetComponent<PrefabOrder>().isFocusedOn = true;
                individualOrdersHolder.transform.GetChild(focusedOrderIndex - 1).GetComponent<PrefabOrder>().isFocusedOn = false;
            }
            else if (gameObject.name == "Player2")
            {
                individualOrdersHolder.transform.GetChild(focusedOrderIndex).GetComponent<PrefabOrderPlayer2>().isFocusedOn = true;
                individualOrdersHolder.transform.GetChild(focusedOrderIndex - 1).GetComponent<PrefabOrderPlayer2>().isFocusedOn = false;
            }
            else if (gameObject.name == "Player3")
            {
                individualOrdersHolder.transform.GetChild(focusedOrderIndex).GetComponent<PrefabOrderPlayer3>().isFocusedOn = true;
                individualOrdersHolder.transform.GetChild(focusedOrderIndex - 1).GetComponent<PrefabOrderPlayer3>().isFocusedOn = false;
            }
            else if (gameObject.name == "Player4")
            {
                individualOrdersHolder.transform.GetChild(focusedOrderIndex).GetComponent<PrefabOrderPlayer4>().isFocusedOn = true;
                individualOrdersHolder.transform.GetChild(focusedOrderIndex - 1).GetComponent<PrefabOrderPlayer4>().isFocusedOn = false;
            }
        }
        //CheckIfPhoneIsActive();
    }

    public void HandleNavigateUILeft()
    {
            if (homeScreen.activeSelf)
            {
                homeScreenScript.dPadLeftPressed = true;
                homeScreenScript.handleGamepadUINavigation();
            }
        else
        {
            Debug.Log("homescreen inactive: insert other dpad navigation code here");
        }
        //CheckIfPhoneIsActive();
    }

    public void HandleNavigateUIRight()
    {
                if (homeScreen.activeSelf)
                {
                    homeScreenScript.dPadRightPressed = true;
                    homeScreenScript.handleGamepadUINavigation();
                }
        else
        {
            Debug.Log("homescreen inactive: insert other dpad navigation code here");
        }
        //CheckIfPhoneIsActive();
    }


    //public void TakePhoneOutOfPocket()
    //{
        

        
    //}

    //public void PutPhoneInPocket()
    //{
    //    //Debug.Log("gameObject.name: " + gameObject.name);
    //    //Debug.Log("phoneGameObject.transform.GetChild(3).gameObject.activeSelf: " + phoneGameObject.transform.GetChild(3).gameObject.activeSelf);
    //    //check if phone or homescreen is active before attempting to put the phone away
    //    //Debug.Log("gameObject.activeInHierarchy: " + gameObject.activeInHierarchy);
    //    //Debug.Log("phone.GetBool('PhoneOn'): " + phone.GetBool("PhoneOn"));

    //    //4 5 11 12... if any of this stuff is open, the home screen should not be active
    //    if (
    //        phoneGameObject.transform.GetChild(4).gameObject.activeSelf ||  //orders menu
    //        phoneGameObject.transform.GetChild(5).gameObject.activeSelf ||  //gps screen 
    //        phoneGameObject.transform.GetChild(11).gameObject.activeSelf ||  //ratings screen
    //        phoneGameObject.transform.GetChild(12).gameObject.activeSelf // scores screen
    //        )
    //    {
    //        phone.SetBool("PhoneOn", false);
    //        phoneGameObject.transform.GetChild(3).gameObject.SetActive(false);
    //    }

    //    if (!phone.GetBool("PhoneOn") || !phoneGameObject.transform.GetChild(3).gameObject.activeSelf/* || !gameObject.activeInHierarchy*/)
    //    {
    //        //Debug.Log("returning from phone off or homescreen inactive check");
    //        return;
    //    }

        
    //}

    public void HandleNavigatePhoneStepIn()
    {
        
        if (!phone.GetBool("PhoneOn"))
        {
            AudioManager.Instance.PlaySoundSFX(phoneInOutAudioClip, gameObject);
            phone.SetBool("PhoneOn", true);
            phoneActive = true;
        }    
        else if (phone.GetBool("PhoneOn") && phoneGameObject.transform.GetChild(3).gameObject.activeSelf)//homescreen
        {
            homeScreenScript.ButtonPressed();
        }    
        else if (phone.GetBool("PhoneOn") && ordersScreen.activeSelf)//ordersScreen
        {
            Debug.Log("inside ordersScreen ActiveSelf check");
            if (gameObject.name == "Player1")
            {
                Debug.Log("inside check for Player1");
                for (int i = 0; i < individualOrdersHolder.transform.childCount; i++)
                {
                    if (individualOrdersHolder.transform.GetChild(i).GetComponent<PrefabOrder>().isFocusedOn)
                    {
                        individualOrdersHolder.transform.GetChild(i).GetComponent<PrefabOrder>().OrderAccepted();
                        return;
                    }
                }
            }
            else if (gameObject.name == "Player2")
            {
                Debug.Log("inside check for Player2");

                for (int i = 0; i < individualOrdersHolder.transform.childCount; i++)
                {
                    if (individualOrdersHolder.transform.GetChild(i).GetComponent<PrefabOrderPlayer2>().isFocusedOn)
                    {
                        individualOrdersHolder.transform.GetChild(i).GetComponent<PrefabOrderPlayer2>().OrderAccepted();
                        return;
                    }
                }
            }
            else if (gameObject.name == "Player3")
            {
                Debug.Log("inside check for Player3");

                for (int i = 0; i < individualOrdersHolder.transform.childCount; i++)
                {
                    if (individualOrdersHolder.transform.GetChild(i).GetComponent<PrefabOrderPlayer3>().isFocusedOn)
                    {
                        individualOrdersHolder.transform.GetChild(i).GetComponent<PrefabOrderPlayer3>().OrderAccepted();
                        return;
                    }
                }
            }
            else if (gameObject.name == "Player4")
            {
                Debug.Log("inside check for Player4");

                for (int i = 0; i < individualOrdersHolder.transform.childCount; i++)
                {
                    if (individualOrdersHolder.transform.GetChild(i).GetComponent<PrefabOrderPlayer4>().isFocusedOn)
                    {
                        individualOrdersHolder.transform.GetChild(i).GetComponent<PrefabOrderPlayer4>().OrderAccepted();
                        return;
                    }
                }
            }

        }
    }   
    
    public void HandleNavigatePhoneStepOut()
    {
        //if phone on and homescreen off, we must be on a different screen, so go back to the homescreen
        if (phone.GetBool("PhoneOn") && !phoneGameObject.transform.GetChild(3).gameObject.activeSelf)
        {
            //go back to home screen
            phoneCanvas.GetComponentInParent<PhoneScript>().HomeScreen.SetActive(true);
            phoneCanvas.GetComponentInParent<PhoneScript>().GPSMenu.SetActive(false);
            phoneCanvas.GetComponentInParent<PhoneScript>().OrdersMenu.SetActive(false);
            phoneCanvas.GetComponentInParent<PhoneScript>().RatingsScreen.SetActive(false);
            phoneCanvas.GetComponentInParent<PhoneScript>().CurrentScores.SetActive(false);
            return;
        }

        //if phone on and homescreen on, put the phone away
        else if (phone.GetBool("PhoneOn") && phoneGameObject.transform.GetChild(3).gameObject.activeSelf)
        {
            //put phone away
            AudioManager.Instance.PlaySoundSFX(phoneInOutAudioClip, gameObject);
            phone.SetBool("PhoneOn", false);
            phoneActive = false;
        }    
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("DaytimeDaNang");
        //Clear bools and static values
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainMenu");
        //Clear bools and static values
    }
    //public void HandleMenuItemSelectButtonPressed()
    //{
    //    //Debug.Log("phoneGameObject.transform.GetChild(3).gameObject.activeSelf: " + phoneGameObject.transform.GetChild(3).gameObject.activeSelf);
    //    //if (!phone.GetBool("PhoneOn"))
    //    //{
    //    //    Debug.Log("returning from button pressed while phone or home screen inactive");
    //    //    return;
    //    //}
        
    //}

    //public void HandleBackToPhoneHomeScreen()
    //{
    //    //Debug.Log("gameObject.name: " + gameObject.name);
    //    //Debug.Log("gameObject.GetComponentInChildren<PhoneScript>(): " + gameObject.GetComponentInChildren<PhoneScript>());
    //    //Debug.Log("gameObject.GetComponentInChildren<PhoneScript>().HomeScreen: " + gameObject.GetComponentInChildren<PhoneScript>().HomeScreen);
    //    //gameObject.GetComponentInChildren<PhoneScript>().HomeScreen.SetActive(true);
    //    //gameObject.GetComponentInChildren<PhoneScript>().GPSMenu.SetActive(false);
    //    //gameObject.GetComponentInChildren<PhoneScript>().OrdersMenu.SetActive(false);
    //    //gameObject.GetComponentInChildren<PhoneScript>().RatingsScreen.SetActive(false);
    //    //gameObject.GetComponentInChildren<PhoneScript>().CurrentScores.SetActive(false);
    //    //gameObject.GetComponentInChildren<PhoneScript>().OrdersBack();
        
        
        
    //}
}
