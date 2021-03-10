using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlagTranslations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Camera menuCamera;
    public GameObject startMenu;
    public InputField ipField;
    public InputField usernameField;
    public GameObject playerUI;

    public Text ipText;
    public Text unameText;

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
        //Clear the error messages
        GameManager.instance.ProcessClientMessage(ClientCodeTranslations.noError);
        
        //Nothing was entered
        if (ipText.text == "" || unameText.text == "")
        {
            Debug.Log("Client did not fill out forms");
            GameManager.instance.ProcessClientMessage(ClientCodeTranslations.badForms);
            return;
        }


        //Disables menu and cameras
        startMenu.SetActive(false);
        ipField.interactable = false;
        usernameField.interactable = false;
        menuCamera.enabled = false;

        //Remove this after dev!
        if (ipText.text == "lh")
        {
            Client.instance.ConnectToServer("127.0.0.1");
        } 
        else
        {
            //Connects to server
            Client.instance.ConnectToServer(ipText.text);
        }

        //Shows player UI
        playerUI.SetActive(true);
    }

    public void RestoreUI()
    {
        startMenu.SetActive(true);
        ipField.interactable = true;
        usernameField.interactable = true;
        menuCamera.enabled = true;
        playerUI.SetActive(false);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }


    //For other scripts to restore the UI
    public static UIManager GetInstance()
    {
        return instance;
    }
}
