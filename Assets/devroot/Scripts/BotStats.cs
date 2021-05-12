using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStats : MonoBehaviour
{
    //This class deals with the bots stats such as health etc.
    public int ammo;
    public int health;
    public float respawnTimer;
    public AudioClip[] deathSounds;

    private SpawnHandler spawnHandler;
    private bool awaitingRespawn;
    private float respawnCachedTimer;

    private int cachedAmmo;
    private int cachedHealth;



    // Start is called before the first frame update
    void Start()
    {
        //Used for respawn timer
        awaitingRespawn = false;

        spawnHandler = FindObjectOfType<SpawnHandler>();

        //Caching values so they can be reset later
        respawnCachedTimer = respawnTimer;
        cachedAmmo = ammo;
        cachedHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        //Triggered on death
        if (awaitingRespawn)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer < 0.2f)
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

    public bool DecreaseHealth(int ht)
    {
        this.health -= ht;
        if (this.health < 1)
        {
            Die();
            return true;
        }

        return false;
    }

    private void Die()
    {
        //Play random death audio
        int randomDeathSoundNum = (int)Random.Range(0, deathSounds.Length);
        AudioSource.PlayClipAtPoint(deathSounds[randomDeathSoundNum], transform.position);

        gameObject.transform.position = new Vector3(0, -3000, 0);
        awaitingRespawn = true;
    }


    private void Respawn()
    {
        //Resets values
        this.health = cachedHealth;
        this.ammo = cachedAmmo;
        awaitingRespawn = false;

        respawnTimer = respawnCachedTimer;
        gameObject.transform.position = spawnHandler.GetRandomGenericSpawn().transform.position;
    }
}
