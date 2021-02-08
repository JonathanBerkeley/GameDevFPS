﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MiscInputListener : MonoBehaviour
{
    
    void Update()
    {
        //For disconnecting using ESC key
        StaticInput.UpdatePauseCheck();
        if (StaticInput.GetPaused())
        {
            SceneManager.LoadScene("MenuScene");

            if (Client.instance != null)
            {
                Debug.Log("DEV -- Remember to repair this");
                //Client.instance.RequestClientDisconnect("BOTH");
            }
        }
    }
}
