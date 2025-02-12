﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{

    private PlayerInput playerInput;
    public ScooterDrive scooterDriveScript;
    public GameObject mainMenuCamera;
    public bool isEnterPressed = false;
    public bool isBackspacePressed = false;
    public GameObject thisPlayersPauseMenu;

    public GameObject creditsPanel;

    public AudioClip phoneButtonPressedAudioClip;


    [SerializeField] private int playerindexTest;

    // Start is called before the first frame update
    private void Awake()
    {
        if (scooterDriveScript == null)
        {
            this.enabled = false;
        }

        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            //int index = playerInput.playerIndex;
            //Debug.Log("index from playerInput component: " + index);
            //var allScooterDriveScrpits = FindObjectsOfType<ScooterDrive>();
            //if (index == 0)
            //{
            //    scooterDriveScript = allScooterDriveScrpits.FirstOrDefault(x => x.playerIndex == index);
            //}
            //else
            //{
            //    scooterDriveScript = allScooterDriveScrpits.FirstOrDefault(x => x.playerIndex == index - 1);
            //}
            //playerindexTest = scooterDriveScript.playerIndex;

        }

        //if (GetComponent<ScooterDrive>().playerIndex == index)
        //{
        //    scooterDriveScript = GetComponent<ScooterDrive>();
        //}
    }

    private void Start()
    {
        //playerInput = GetComponent<PlayerInput>();
        //if (playerInput != null)
        //{
        //    int index = playerInput.playerIndex;
        //    var allScooterDriveScrpits = FindObjectsOfType<ScooterDrive>();
        //    if (index == 0)
        //    {
        //        scooterDriveScript = allScooterDriveScrpits.FirstOrDefault(x => x.playerIndex == index);
        //    }
        //    else
        //    {
        //        scooterDriveScript = allScooterDriveScrpits.FirstOrDefault(x => x.playerIndex == index - 1);
        //    }
            //playerindexTest = scooterDriveScript.playerIndex;

        
    }

    public void OnBrakeCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            if (scooterDriveScript.currentSpeed > 0)
            {
                scooterDriveScript.isBraking = true;
                scooterDriveScript.isReversingCompleted = true;
                scooterDriveScript.isReversing = false;
                scooterDriveScript.isBrakingCompleted = false;
            }
            else
            {
                scooterDriveScript.isReversing = true;
                scooterDriveScript.isBraking = false;
                scooterDriveScript.isBrakingCompleted = true;
                scooterDriveScript.isReversingCompleted = false;
            }

        }
        else if (context.canceled)
        {
            scooterDriveScript.isBraking = false;
            scooterDriveScript.isBrakingCompleted = true;
            scooterDriveScript.isReversing = false;
            scooterDriveScript.isReversingCompleted = true;
        }
    }
    public void OnTurnRightCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            scooterDriveScript.turnRight = true;
        }
        else if (context.canceled)
        {
            scooterDriveScript.turnRight = false;
        }
    }
    public void OnTurnLeftCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            scooterDriveScript.turnLeft = true;
        }
        else if (context.canceled)
        {
            scooterDriveScript.turnLeft = false;
        }
    }
    public void OnAccelerateCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            scooterDriveScript.isAccelerating = true;
        }
        else if (context.canceled)
        {
            scooterDriveScript.isAccelerating = false;
            scooterDriveScript.acceleratingCompleted = true;
        }
    }

    public void OnNavigateUIUpCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            scooterDriveScript.HandleNavigateUIUp();
        }
    }
    public void OnNavigateUIRightCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            scooterDriveScript.HandleNavigateUIRight();
        }
    }
    public void OnNavigateUIDownCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            scooterDriveScript.HandleNavigateUIDown();
        }
    }
    public void OnNavigateUILeftCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            scooterDriveScript.HandleNavigateUILeft();
        }
    }
    public void OnNavigatePhoneStepInCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            scooterDriveScript.HandleNavigatePhoneStepIn();
        }
    }

    public void OnNavigatePhoneStepOutCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            scooterDriveScript.HandleNavigatePhoneStepOut();
        }
    }

    public void OnIncreasePlayerCountCallbackInputs(CallbackContext context)
    {
        if (context.phase.ToString() == "Started" || context.phase.ToString() == "Canceled")
        {
            return;
        }
        else if (context.performed)
        {
            //AudioManager.Instance.PlaySoundSFX(phoneButtonPressedAudioClip, gameObject, volume: 0.5f);

            gameObject.GetComponent<PlayerCharacter>().HandleRightShoulderButton();
        }
    }

    public void OnDecreasePlayerCountCallbackInputs(CallbackContext context)
    {
        if (context.phase.ToString() == "Started" || context.phase.ToString() == "Canceled")
        {
            return;
        }
        else if (context.performed)
        {
            //AudioManager.Instance.PlaySoundSFX(phoneButtonPressedAudioClip, gameObject, volume: 0.5f);

            gameObject.GetComponent<PlayerCharacter>().HandleLeftShoulderButton();
        }
    }

    public void OnCharacterSelectLeftCallbackBindings(CallbackContext context)
    {
        Debug.Log("inside character select left");
        if (context.phase.ToString() == "Started" || context.phase.ToString() == "Canceled")
        {
            return;
        }
        else if (context.performed)
        {
            gameObject.GetComponent<PlayerCharacter>().HandleLeftAnalogPressedLeft();
        }
    }

    public void OnCharacterSelectRightCallbackBindings(CallbackContext context)
    {
        Debug.Log("inside character select right");

        if (context.phase.ToString() == "Started" || context.phase.ToString() == "Canceled")
        {
            return;
        }
        else if (context.performed)
        {
            gameObject.GetComponent<PlayerCharacter>().HandleLeftAnalogPressedRight();
        }
    }

    public void OnStartGameCallbackBindings(CallbackContext context)
    {
        //if (context.phase.ToString() == "Started" || context.phase.ToString() == "Canceled")
        //{
        //    return;
        //}

        if (context.performed)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) && !isEnterPressed)
            {
                isEnterPressed = true;
                // TODO
                // your logic here when button pressed
                GameObject.Find("Main Camera").GetComponent<MainMenu>().HandleStartButton();
                return;
            }

            else if (Input.GetKeyUp(KeyCode.KeypadEnter) && isEnterPressed)
            {
                isEnterPressed = false;
                // TODO
                // your logic here when button released
                return;
            }
            GameObject.Find("Main Camera").GetComponent<MainMenu>().HandleStartButton();
        }
    }

    public void OnPauseMenuCallbackBindings(CallbackContext context)
    {
        //if (context.phase.ToString() == "Started" || context.phase.ToString() == "Canceled")
        //{
        //    return;
        //}

        if (context.performed)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) && !isEnterPressed)
            {
                isEnterPressed = true;
                // TODO
                // your logic here when button pressed
                if (!thisPlayersPauseMenu.activeSelf)
                {
                    thisPlayersPauseMenu.SetActive(true);
                }
                else if (thisPlayersPauseMenu.activeSelf)
                {
                    thisPlayersPauseMenu.SetActive(false);
                }
                return;
            }

            else if (Input.GetKeyUp(KeyCode.KeypadEnter) && isEnterPressed)
            {
                isEnterPressed = false;
                // TODO
                // your logic here when button released
                return;
            }

            Debug.Log("start button for pause menu being recognized");
            if (!thisPlayersPauseMenu.activeSelf)
            {
                Debug.Log("inside if pause menu is on check");
                thisPlayersPauseMenu.SetActive(true);
                GameObject.Find("GameManager").GetComponent<GameManager>().GameIsPaused = true;
            }
            else if (thisPlayersPauseMenu.activeSelf)
            {
                Debug.Log("inside if pause menu is on check");
                thisPlayersPauseMenu.SetActive(false);
                GameObject.Find("GameManager").GetComponent<GameManager>().GameIsPaused = false;
            }
        }
    }

    public void OnBackToTitleScreenCallbackBindings(CallbackContext context)
    {
        if (context.performed)
        {
            if (Input.GetKeyDown(KeyCode.Backspace) && !isBackspacePressed)
            {

                isBackspacePressed = true;
                // TODO
                // your logic here when button pressed
                GameObject.Find("Main Camera").GetComponent<MainMenu>().gameCreateBack();
                return;
            }

            else if (Input.GetKeyUp(KeyCode.Backspace) && isBackspacePressed)
            {
                isBackspacePressed = false;
                // TODO
                // your logic here when button released
                return;
            }
            GameObject.Find("Main Camera").GetComponent<MainMenu>().gameCreateBack();
        }
    }

    public void OnThrowWeaponCallbackInputs(CallbackContext context)
    {
        if (context.phase.ToString() == "Started" || context.phase.ToString() == "Canceled")
        {
            return;
        }
        else if (context.performed)
        {
            gameObject.GetComponent<ScooterDrive>().HandleRightTrigger();
        }
    }

    public void OnClearCameraObstructionCallbackInputs(CallbackContext context)
    {
        if (context.phase.ToString() == "Started" || context.phase.ToString() == "Canceled")
        {
            return;
        }
        else if (context.performed)
        {
            gameObject.GetComponent<ScooterDrive>().HandleLeftTrigger();
        }
    }

    public void OnCreditsCallbackInputs(CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("credits callbacks called");
            if (!creditsPanel.activeSelf)
            {
                creditsPanel.SetActive(true);
            }
            else
            {
                creditsPanel.SetActive(false);
            }
        }
    }
}
