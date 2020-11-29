using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Allows null
    public int? health;
    public int? ammo;

    private void Awake()
    {
        if (health == null) 
        {
            health = 100;
        }
        if (ammo == null)
        {
            ammo = 20;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

}
