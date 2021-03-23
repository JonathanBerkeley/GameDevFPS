using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//PlayerStats script for managing player stats such as health, ammo etc
public class PlayerStats : MonoBehaviour
{
    //This class deals with the players stats such as health etc.
    public int ammo;
    public int health;
    public float respawnTimer;

    public RawImage healthImage;
    public Texture[] healthTextures;
    public Text ammoText;
    public GameObject gameLogic;
    public GameObject deathPanel;
    public GameObject uiCanvas;
    public Text respawnText;

    public Component[] disableOnRespawnAwait;

    private SpawnHandler spawnHandler;
    private bool awaitingRespawn;
    private float respawnCachedTimer;

    private int cachedAmmo;
    private int cachedHealth;

    private void Start()
    {
        //Sets the text to initial values
        ammoText.text = ammo.ToString();

        healthImage.texture = healthTextures[0];
        spawnHandler = gameLogic.GetComponent<SpawnHandler>();

        //Used for respawn timer
        awaitingRespawn = false;

        //Caching values so they can be reset later
        respawnCachedTimer = respawnTimer;
        cachedAmmo = ammo;
        cachedHealth = health;

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


        //Triggered on death
        if (awaitingRespawn)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer > 0.2f)
            {
                respawnText.text = "Respawn in (" + (int)(respawnTimer) + ")";
            } else
            {
                Respawn();
            }
        }
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
            Die();
        }
    }

    //Code triggered on death
    private void Die()
    {
        gameObject.transform.position = new Vector3(0, -3000, 0);
        deathPanel.SetActive(true);
        uiCanvas.SetActive(false);

        //Stops select scripts on player
        foreach (MonoBehaviour mb in disableOnRespawnAwait)
        {
            mb.enabled = false;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        awaitingRespawn = true;
    }

    private void Respawn()
    {
        //Resets values
        this.health = cachedHealth;
        this.ammo = cachedAmmo;
        awaitingRespawn = false;
        uiCanvas.SetActive(true);
        deathPanel.SetActive(false);

        foreach (MonoBehaviour mb in disableOnRespawnAwait)
        {
            mb.enabled = true;
        }
        Cursor.visible = false;
        respawnTimer = respawnCachedTimer;

        gameObject.transform.position = spawnHandler.GetRandomGenericSpawn().transform.position;
    }
}
