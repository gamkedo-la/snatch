﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject[] characters;
    public string[] characterName;
    public int characterCount = 0;
    public Text characterNameString;

    public bool player1, player2, player3, player4;

    public GameObject mainCamera;
    MainMenu mainMenuScript;

    private void Awake()
    {
        mainMenuScript = mainCamera.GetComponent<MainMenu>();
    }

    public void Left()
    {
        characters[characterCount].SetActive(false);
        characterCount--;
        if (characterCount < 0 )
        {
            characterCount = characters.Length -1;
        }
        characters[characterCount].SetActive(true);
        if (player1)
        {
            MainMenu.player1Character = characterCount;
        }
        if (player2)
        {
            MainMenu.player2Character = characterCount;
        }
        if (player3)
        {
            MainMenu.player3Character = characterCount;
        }
        if (player4)
        {
            MainMenu.player4Character = characterCount;
        }
    }

    public void Right()
    {
        characters[characterCount].SetActive(false);
        characterCount++;
        if (characterCount == characters.Length)
        {
            characterCount = 0;
        }
        characters[characterCount].SetActive(true);
        if (player1)
        {
            MainMenu.player1Character = characterCount;
        }
        if (player2)
        {
            MainMenu.player2Character = characterCount;
        }
        if (player3)
        {
            MainMenu.player3Character = characterCount;
        }
        if (player4)
        {
            MainMenu.player4Character = characterCount;
        }
    }

    private void Update()
    {
        characterNameString.text = characterName[characterCount];
    }

    public void HandleLeftShoulderButton()
    {
        Debug.Log("HandleLeftShoulderButton triggered");
        mainMenuScript.decreasePlayerCount();
    }

    public void HandleRightShoulderButton()
    {
        Debug.Log("HandleLeftShoulderButton triggered");

        mainMenuScript.increasePlayerCount();
    }

    public void HandleLeftAnalogPressedRight()
    {
        Right();
    }

    public void HandleLeftAnalogPressedLeft()
    {
        Left();
    }
}
