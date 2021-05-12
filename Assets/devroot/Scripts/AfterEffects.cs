using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AfterEffects : MonoBehaviour
{
    public static AfterEffects instance;
    [Range(0.01f, 0.99f)]
    public float minVignetteIntensity, maxVignetteIntensity, vignetteRate;

    private PostProcessVolume volume;
    private Vignette vignette;
    private bool animationFlip;
    private void Awake()
    {
        // Make this a singleton class
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        /*
        vignette = ScriptableObject.CreateInstance<Vignette>();
        vignette.enabled.Override(true);
        vignette.intensity.Override(1.0f);
        */
        animationFlip = false;
        vignetteRate /= 100;
        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        if (vignette.enabled.value)
        {
            if (!animationFlip && vignette.intensity.value < maxVignetteIntensity)
            {
                vignette.intensity.value += vignetteRate;
            }
            else
            {
                animationFlip = true;
            }
            
            if (animationFlip && vignette.intensity.value > minVignetteIntensity)
            {
                vignette.intensity.value -= vignetteRate;
            }
            else
            {
                animationFlip = false;
            }
        }
    }

    public void SetVignette(bool _status)
    {
        vignette.enabled.value = _status;
    }
}
