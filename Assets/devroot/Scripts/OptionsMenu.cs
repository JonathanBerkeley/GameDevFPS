using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public Button[] mainTabs;
    public int defaultSelected = 0;
    public GameObject[] innerMenus;

    private int _currentlySelected;
    void Start()
    {
        if (defaultSelected >= mainTabs.Length)
        {
            defaultSelected = 0;
        }
        _currentlySelected = defaultSelected;
        
        if (innerMenus.Length < mainTabs.Length)
        {
            Debug.LogError("Options menu - mismatch of tabs to menus");
        }

        EnableSelected();
    }

    void Update()
    {
        mainTabs[_currentlySelected].Select();
        
    }

    public void SetSelected(int _select)
    {
        _currentlySelected = _select;
        EnableSelected();

        if (_currentlySelected == 1 && innerMenus[1] != null)
            innerMenus[1].GetComponentInChildren<Slider>().value = GlobalAudioReference.instance.GetMasterVolume();
    }

    //Referenced dynamically by volume slider
    public void SetMasterVolume(float _volume)
    {
        if (GlobalAudioReference.instance != null)
            GlobalAudioReference.instance.SetMasterVolume(_volume);
    }
    
    private void EnableSelected()
    {
        for (int i = 0; i < innerMenus.Length; ++i)
        {
            if (i != _currentlySelected)
            {
                innerMenus[i].SetActive(false);
            }
            else
            {
                innerMenus[i].SetActive(true);
            }
        }
    }
}
