using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigureScene : MonoBehaviour
{
    //This is for scene specific settings changes
    void Start()
    {
        RenderSettings.fog = true;
    }
}
