using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for transferring settings data across scenes
public static class SettingsData
{
    private static int botsDesired = 0;
    private static bool customBotOption = false;
    private static bool fogDesired = true;
    

    //Sets
    public static void SetBotsDesired(int b)
    {
        customBotOption = true;
        botsDesired = b;
    }
    
    public static void SetFogDesired(bool f)
    {
        fogDesired = f;
    }
    private static void SetCustomBotOption(bool cbo)
    {
        customBotOption = cbo;
    }


    //Gets
    public static int GetBotsDesired()
    {
        return botsDesired;
    }

    public static bool GetFogDesired()
    {
        return fogDesired;
    }

    //Returns false if no custom bot option being used
    public static bool GetCustomBotOption()
    {
        return customBotOption;
    }
}
