using System.Collections;
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
            Client.instance.RequestClientDisconnect("BOTH");
        }
    }
}
