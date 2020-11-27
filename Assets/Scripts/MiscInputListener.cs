using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MiscInputListener : MonoBehaviour
{
    void Update()
    {
        StaticInput.UpdatePauseCheck();
        if (StaticInput.GetPaused())
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
