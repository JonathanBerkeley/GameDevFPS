using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Multiplayer script for managing player stats such as health, ammo etc

//This script is very similar to PlayerStats, except it gets components at runtime
public class MultiplayerStats : MonoBehaviour
{
    //This class deals with the players stats such as health etc.
    public int ammo;
    public int health;
    public float respawnTimer;

    public Texture[] healthTextures;

    public Component[] disableOnRespawnAwait;

    private SpawnHandler spawnHandler;
    private bool awaitingRespawn;
    private float respawnCachedTimer;

    //Changed to private from PlayerStats for multiplayer runtime component retrieval
    private RawImage healthImage;
    private Text ammoText;
    private GameObject gameLogic;
    private GameObject deathPanel;
    private GameObject uiCanvas;
    private Text respawnText;

    private int cachedAmmo;
    private int cachedHealth;
    private HoldReferences _disabledComponents;

    private void Awake()
    {
        //Runtime retrieval of components for multiplayer prefabs

        //Image
        healthImage = GameObject.FindGameObjectWithTag("MP_HealthImage").GetComponent<RawImage>();

        //Text
        ammoText = GameObject.FindGameObjectWithTag("MP_AmmoText").GetComponent<Text>();

        //Gameobjects
        gameLogic = GameObject.FindGameObjectWithTag("MP_GameLogic");
        deathPanel = GameObject.FindGameObjectWithTag("MP_DeathPanel");
        uiCanvas = GameObject.FindGameObjectWithTag("MP_UiCanvas");
        
    }

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

        _disabledComponents = GameObject.FindGameObjectWithTag("MP_DisableComponents").GetComponent<HoldReferences>();

    }

    private void Update()
    {
        if (health > 80)
        {
            healthImage.texture = healthTextures[0];
        }
        else if (health > 60)
        {
            healthImage.texture = healthTextures[1];
        }
        else if (health > 40)
        {
            healthImage.texture = healthTextures[2];
        }
        else
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
            }
            else
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
        }
        else
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
        uiCanvas.SetActive(false);

        /*
        foreach (GameObject go in _disabledComponents.GetObjectReferences())
        {
            go.SetActive(true);
        }
        */

        _disabledComponents.GetObjectReferenceByTag("MP_DeathPanel").SetActive(true);


        respawnText = _disabledComponents.GetObjectReferenceByTag("MP_RespawnText").GetComponent<Text>();

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
        /*
        foreach (GameObject go in _disabledComponents.GetObjectReferences())
        {
            go.SetActive(false);
        }
        */

        _disabledComponents.GetObjectReferenceByTag("MP_DeathPanel").SetActive(false);

        foreach (MonoBehaviour mb in disableOnRespawnAwait)
        {
            mb.enabled = true;
        }
        Cursor.visible = false;
        respawnTimer = respawnCachedTimer;

        gameObject.transform.position = spawnHandler.GetRandomGenericSpawn().transform.position;
    }
}
