using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //This class deals with the players stats such as health etc.
    public int ammo;
    public int health;

    public RawImage healthImage;
    public Texture[] healthTextures;
    public Text ammoText;

    private void Start()
    {
        //Sets the text to initial values
        ammoText.text = ammo.ToString();
        healthImage.texture = healthTextures[0];
    }

    private void Update()
    {
        if (health > 80)
        {
            healthImage.texture = healthTextures[0];
        } else if (health > 60)
        {
            healthImage.texture = healthTextures[1];
        } else if (health > 40)
        {
            healthImage.texture = healthTextures[2];
        } else
        {
            healthImage.texture = healthTextures[3];
        }

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

    public void IncreaseAmmo(int am)
    {
        this.ammo += am;
    }

    public void IncreaseHealth(int ht)
    {
        int calcHealth = this.health + ht;
        if (calcHealth >= 100)
        {
            this.health = 100;
        } else
        {
            this.health += ht;
        }
    }

    public void DecreaseHealth(int ht)
    {
        this.health -= ht;
        if (this.health < 1)
        {
            //Die code
        }
    }

}
