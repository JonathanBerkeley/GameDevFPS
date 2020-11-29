using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //Allows null
    public int health;
    public int ammo;

    public Text healthText;
    public Text ammoText;

    private void Awake()
    {
    }

    private void Start()
    {
        //Sets the text to initial values
        healthText.text = health.ToString();
        ammoText.text = ammo.ToString();
    }

    private void Update()
    {
        healthText.text = health.ToString();
        ammoText.text = ammo.ToString();
    }


    //Get info for other classes
    public int GetAmmo()
    {
        return this.ammo;
    }

    public int GetHealth()
    {
        return this.health;
    }


    //Sets for other classes to use
    public void SetAmmo(int am)
    {
        this.ammo = am;
    }

}
