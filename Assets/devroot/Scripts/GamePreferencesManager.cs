using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to maintain game settings through restarts etc
//Made following Unity official tutorial https://www.youtube.com/watch?v=uD7y4T4PVk0
public class GamePreferencesManager : MonoBehaviour
{
    const string MasterVolumeKey = "MasterVolume";
    const string EffectsVolumeKey = "EffectsVolume";
    const string FullscreenKey = "Fullscreen";
    const string FogKey = "Fog";
    const string ResolutionKey = "Resolution";

    private void Start()
    {
        LoadPrefs();
    }

    private void OnApplicationQuit()
    {
        SavePrefs();
    }

    public void SavePrefs()
    {
        if (!GlobalAudioReference.instance)
            return;

        PlayerPrefs.SetFloat(MasterVolumeKey, GlobalAudioReference.instance.GetMasterVolume());

        if (!VideoSettings.instance)
            return;

        
        PlayerPrefs.SetInt(FullscreenKey, VideoSettings.instance.GetFullscreen());
        PlayerPrefs.SetInt(ResolutionKey, VideoSettings.instance.GetResolution());
        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {
        if (!GlobalAudioReference.instance)
            return;

        GlobalAudioReference.instance.SetMasterVolume(PlayerPrefs.GetFloat(MasterVolumeKey, 0.5f));

        if (!VideoSettings.instance)
            return;

        bool fsEnable = PlayerPrefs.GetInt(FullscreenKey, 0) == 0 ? false : true;

        VideoSettings.instance.ChangeFullscreen(fsEnable);
        VideoSettings.instance.ChangeResolution(PlayerPrefs.GetInt(ResolutionKey, 0));

    }
}
