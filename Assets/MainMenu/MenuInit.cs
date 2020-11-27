using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuInit : MonoBehaviour
{
    public Button[] uiButtons;
    public Button[] settingsButtons;
    public GameObject mainMenu;
    public GameObject settingsMenu;

    //Colours for fog true/false
    private Color buttonColorFalse = new Color32(164, 35, 35, 200);
    private Color buttonColorTrue = new Color32(75, 181, 75, 200);

    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);

        if (uiButtons.Length == 3)
        {
            //Sets up event listeners
            Button btn = uiButtons[0].GetComponent<Button>();
            btn.onClick.AddListener(PlayButtonClicked);
            
            btn = uiButtons[1].GetComponent<Button>();
            btn.onClick.AddListener(SettingsButtonClicked);

            btn = uiButtons[2].GetComponent<Button>();
            btn.onClick.AddListener(ExitButtonClicked);
        }

        if (settingsButtons.Length == 3)
        {
            Button sbtn = settingsButtons[0].GetComponent<Button>();
            sbtn.onClick.AddListener(BotsButtonClicked);

            sbtn = settingsButtons[1].GetComponent<Button>();
            sbtn.onClick.AddListener(FogButtonClicked);
            
            sbtn = settingsButtons[2].GetComponent<Button>();
            sbtn.onClick.AddListener(BackButtonClicked);
        }
    }

    void Update()
    {
        //Shows cursor when returning from play
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //Menu button functionality below

    void PlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    void SettingsButtonClicked()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    //Exit when button clicked. Also supports stopping editor.
    //Interesting reflection code for forcing editor to stop adopted from below link
    //http://answers.unity.com/answers/599597/view.html
    //This should also work for webplayer, whereas Application.Quit() does not.
    void ExitButtonClicked()
    {
        if (Application.isEditor)
        {
            Type t = null;
            foreach (var assm in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = assm.GetType("UnityEditor.EditorApplication");
                if (t != null)
                {
                    t.GetProperty("isPlaying").SetValue(null, false, null);
                    break;
                }
            }
        } else
        {
            Application.Quit();
        }
    }


    //Settings menu buttons below

    void BotsButtonClicked()
    {
        Text buttonText = settingsButtons[0].GetComponentInChildren<Text>();
        if (SettingsData.GetBotsDesired() < 7)
        {
            SettingsData.SetBotsDesired(SettingsData.GetBotsDesired() + 1);
        } else
        {
            SettingsData.SetBotsDesired(0);
        }
        buttonText.text = "Bots " + SettingsData.GetBotsDesired();
    }
    
    void FogButtonClicked()
    {
        
        //Flips current setting
        SettingsData.SetFogDesired(!(SettingsData.GetFogDesired()));

        if (SettingsData.GetFogDesired())
        {
            settingsButtons[1].GetComponent<Image>().color = buttonColorTrue;
        } else
        {
            settingsButtons[1].GetComponent<Image>().color = buttonColorFalse;
        }
    }

    void BackButtonClicked()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
