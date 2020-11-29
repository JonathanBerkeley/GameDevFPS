﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Camera menuCamera;
    public GameObject startMenu;
    public InputField usernameField;

    private void Awake()
    {
        //Make this a singleton class
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already in existence, destroying self");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        //Disables menu and cameras
        startMenu.SetActive(false);
        usernameField.interactable = false;
        menuCamera.enabled = false;

        //Connects to server
        Client.instance.ConnectToServer();
    }
}
