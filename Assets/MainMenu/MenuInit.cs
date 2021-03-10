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
    public Button[] playButtons;

    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject playMenu;

    public AudioClip clickAudio;
    public float menuAudioVolume;

    //Colours for fog true/false
    private Color buttonColorFalse = new Color32(164, 35, 35, 200);
    private Color buttonColorTrue = new Color32(75, 181, 75, 200);

    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        playMenu.SetActive(false);

        //Sets up event listeners for main menu
        if (uiButtons.Length == 3)
        {
            Button btn = uiButtons[0].GetComponent<Button>();
            btn.onClick.AddListener(PlayButtonClicked);
            
            btn = uiButtons[1].GetComponent<Button>();
            btn.onClick.AddListener(SettingsButtonClicked);

            btn = uiButtons[2].GetComponent<Button>();
            btn.onClick.AddListener(ExitButtonClicked);
        }

        //Set up listeners for settings menu
        if (settingsButtons.Length == 3)
        {
            Button sbtn = settingsButtons[0].GetComponent<Button>();
            sbtn.onClick.AddListener(BotsButtonClicked);

            sbtn = settingsButtons[1].GetComponent<Button>();
            sbtn.onClick.AddListener(FogButtonClicked);
            
            sbtn = settingsButtons[2].GetComponent<Button>();
            sbtn.onClick.AddListener(BackButtonClicked);
        }

        //Set up listeners for play menu
        if (playButtons.Length == 3)
        {
            Button pbtn = playButtons[0].GetComponent<Button>();
            pbtn.onClick.AddListener(Multiplayer);

            pbtn = playButtons[1].GetComponent<Button>();
            pbtn.onClick.AddListener(Singleplayer);

            pbtn = playButtons[2].GetComponent<Button>();
            pbtn.onClick.AddListener(BackButtonClicked);
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
        MenuClickAudio();
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    void SettingsButtonClicked()
    {
        MenuClickAudio();
        mainMenu.SetActive(false);
        playMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    //Exit when button clicked. Also supports stopping editor.
    //Interesting reflection code for forcing editor to stop adopted from below link
    //http://answers.unity.com/answers/599597/view.html
    //This should also work for webplayer, whereas Application.Quit() does not.
    void ExitButtonClicked()
    {
        MenuClickAudio();
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
        }
        else
        {
            Application.Quit();
        }
    }


    //Settings menu buttons below

    void BotsButtonClicked()
    {
        MenuClickAudio();
        Text buttonText = settingsButtons[0].GetComponentInChildren<Text>();
        if (SettingsData.GetBotsDesired() < 7)
        {
            SettingsData.SetBotsDesired(SettingsData.GetBotsDesired() + 1);
        }
        else
        {
            SettingsData.SetBotsDesired(0);
        }
        buttonText.text = "Bots " + SettingsData.GetBotsDesired();
    }
    
    void FogButtonClicked()
    {
        MenuClickAudio();
        //Flips current setting
        SettingsData.SetFogDesired(!(SettingsData.GetFogDesired()));

        if (SettingsData.GetFogDesired())
        {
            settingsButtons[1].GetComponent<Image>().color = buttonColorTrue;
        }
        else
        {
            settingsButtons[1].GetComponent<Image>().color = buttonColorFalse;
        }
    }

    void BackButtonClicked()
    {
        MenuClickAudio();
        settingsMenu.SetActive(false);
        playMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    //Play menu buttons below
    void Multiplayer()
    {
        MenuClickAudio();
        //Hides all the menus
        settingsMenu.SetActive(false);
        mainMenu.SetActive(false);
        playMenu.SetActive(false);

        SceneManager.LoadScene("MultiScene");
    }    
    
    void Singleplayer()
    {
        MenuClickAudio();
        //Hides all the menus
        settingsMenu.SetActive(false);
        mainMenu.SetActive(false);
        playMenu.SetActive(false);

        SceneManager.LoadScene("GameScene");
    }


    //Audio on menu
    void MenuClickAudio()
    {
        //Figures out menu camera position and plays sound at that location
        Vector3 currentCameraPosition = Camera.main.transform.position;
        currentCameraPosition = (0.9f * currentCameraPosition) + (0.1f * transform.position);
        AudioSource.PlayClipAtPoint(clickAudio, currentCameraPosition, menuAudioVolume);
    }

    
}
