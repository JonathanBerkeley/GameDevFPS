using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For skins on characters
public class RandomMat : MonoBehaviour
{
    public Material[] mats;
    [Header("Enable to enable randomised colours")]
    public bool recolour = false;
    [Header("Recolour selects from presets, enable to generate new colours")]
    public bool trueRandom = false;

    private Color[] colours = { Color.red, Color.white, Color.blue, 
        Color.yellow, Color.green, Color.magenta, Color.clear };
    
    void Awake()
    {
        if (!recolour)
            colours = null;
    }

    void Start()
    {
        Renderer objectRenderer = gameObject.GetComponent<Renderer>();
        // Chooses random material from preset list
        objectRenderer.material = mats[Random.Range(0, mats.Length)];

        if (recolour)
        {
            if (!trueRandom && colours != null)
                objectRenderer.material.color = colours[Random.Range(0, colours.Length)];
            else if (trueRandom)
            {
                float oneUnit = 0.00392156862f; //This is just 1 / 255, representing 1 unit on a 255 scale.

                //Generate a random RGB colour
                objectRenderer.material.color = new Color(
                        Random.Range(0, 256) * oneUnit,
                        Random.Range(0, 256) * oneUnit,
                        Random.Range(0, 256) * oneUnit
                    );

                
            }
        }
        
    }

}
