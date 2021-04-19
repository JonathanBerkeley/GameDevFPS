using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility to disable components by script
public class DisableOn : MonoBehaviour
{
    public static DisableOn instance;
    public Component[] toDisable;

    private void Awake()
    {
        instance = this;
    }

    public void DisableList()
    {
        foreach (Component c in toDisable)
        {
            c.gameObject.SetActive(true);
        }
    }

    public void EnableList()
    {
        foreach (Component c in toDisable)
        {
            c.gameObject.SetActive(false);
        }
    }
}
