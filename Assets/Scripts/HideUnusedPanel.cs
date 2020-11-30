using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility for hiding things on startup
//Specifically used for hiding the death panel when starting
public class HideUnusedPanel : MonoBehaviour
{
    public GameObject[] unused;
    void Start()
    {
        foreach (GameObject go in unused)
        {
            go.SetActive(false);
        }
    }
}
