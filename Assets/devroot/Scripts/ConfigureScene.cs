using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigureScene : MonoBehaviour
{
    public bool forceNoFog;
    public bool forceFog;

    //This is for scene specific settings changes
    void Start()
    {
        //System overrides users settings if set, forceNoFog beats forceFog
        if (forceNoFog)
        {
            RenderSettings.fog = false;
        } else
        {
            if (forceFog)
            {
                RenderSettings.fog = true;
            } else
            {
                RenderSettings.fog = SettingsData.GetFogDesired();
            }
        }

        
        
    }
}
