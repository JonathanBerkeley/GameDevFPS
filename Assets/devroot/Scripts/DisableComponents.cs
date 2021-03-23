using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For disabling components on startup
public class DisableComponents : MonoBehaviour
{
    public Component[] components;

    private void Start()
    {
        foreach (Component c in components)
        {
            c.gameObject.SetActive(false);
        }
    }

    public void EnableDisabledComponents()
    {
        foreach (Component c in components)
        {
            c.gameObject.SetActive(true);
        }
    }
}
