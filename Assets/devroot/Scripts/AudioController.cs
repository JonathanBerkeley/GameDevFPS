using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float audioVolume;

    void Awake()
    {
        AudioListener.volume = audioVolume;
    }
}
