using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for main menu music
public class MusicHandler : MonoBehaviour
{
    public static bool MusicSetting;
    public AudioClip[] mainMenuMusic;

    private AudioListener listener;
    private AudioClip track;
    private IEnumerator nextTrack;

    void Start()
    {
        listener = FindObjectOfType<AudioListener>();
        if (MusicSetting)
            PlayTrack();
    }

    private void PlayTrack()
    {
        track = mainMenuMusic[Random.Range(0, mainMenuMusic.Length)];
        if (GlobalAudioReference.instance != null)
            AudioSource.PlayClipAtPoint(track, listener.transform.position, GlobalAudioReference.masterVolume);
        else
        {
            try
            {
                AudioSource.PlayClipAtPoint(track, listener.transform.position, 0.5f);
            } catch { } // Required due exception in editor when destroying this object
        }

        nextTrack = NextTrack(track.length);
        StartCoroutine(nextTrack);
    }

    // Plays a new track when last one is finished
    IEnumerator NextTrack(float _time)
    {
        yield return new WaitForSeconds(_time);
        PlayTrack();
    }

    // Called by UI
    public void SetMusicStatus(bool _status)
    {
        MusicSetting = _status;
        if (!_status && nextTrack != null)
        {
            StopCoroutine(nextTrack);
            StopMusic();
        }
        else
        {
            PlayTrack();
        }
    }

    // Stops music from playing on menu
    public void StopMusic()
    {
        foreach (AudioSource src in FindObjectsOfType<AudioSource>())
        {
            src.Stop();
        }
    }
}
