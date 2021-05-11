using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioClip[] mainMenuMusic;

    private AudioListener listener;
    void Start()
    {
        listener = GameObject.FindObjectOfType<AudioListener>();
        PlayTrack();
    }

    private void PlayTrack()
    {
        AudioClip track = mainMenuMusic[Random.Range(0, mainMenuMusic.Length)];
        if (GlobalAudioReference.instance != null)
            AudioSource.PlayClipAtPoint(track, listener.transform.position, GlobalAudioReference.masterVolume);
        else
            AudioSource.PlayClipAtPoint(track, listener.transform.position, 0.5f);
        StartCoroutine(NextTrack(track.length));
    }

    IEnumerator NextTrack(float _time)
    {
        yield return new WaitForSeconds(_time);
        PlayTrack();
    }

    public void StopMusic()
    {

    }
}
