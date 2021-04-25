using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Multiplayer variant of projectile script
public class MultiplayerProjectile : MonoBehaviour
{
    public float delay = 3.0f;
    public float blastRadius = 5.0f;
    public float blastForce = 2000.0f;
    public GameObject explosionEffect;
    public AudioClip explosionAudio;

    private int id;
    private float countdown;
    private bool exploded;
    private GameObject _self;
    private Rigidbody _selfBody;

    //Multiplayer variables
    private Vector3 location;
    private Quaternion rotation;
    void Start()
    {
        _self = gameObject;
        _selfBody = _self.GetComponent<Rigidbody>();
        countdown = delay;
    }

    private void Awake()
    {
        location = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
    }

    void Update()
    {
        //Countdown causes it to explode if flying into abyss too long
        countdown -= Time.deltaTime;
        if (countdown <= 0.0f && !exploded)
        {
            Explode();
            exploded = true;
        }
    }

    void Explode()
    {
        //Show explosion effect
        GameObject explosionObject = (GameObject)Instantiate(explosionEffect, transform.position, transform.rotation);

        //Play explosion audio
        AudioSource.PlayClipAtPoint(explosionAudio, transform.position, GlobalAudioReference.instance.GetEffectsVolume());

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);


        //Affect nearby objects
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && rb != _selfBody)
            {
                rb.AddExplosionForce(blastForce, transform.position, blastRadius);

                //Explosion damage based on distance from explosion
                float distanceFromExplosion = Vector3.Distance(transform.position, rb.transform.position);
                GameObject _playerHit = rb.transform.root.gameObject;
                int _damage = 0;

                if (PlayerID.GetIDByGameObject(rb.gameObject) != this.id)
                {
                    if (distanceFromExplosion < 2.0f)
                    {
                        _damage = 60;
                    }
                    else if (distanceFromExplosion < 5.0f)
                    {
                        _damage = 40;
                    }
                    else if (distanceFromExplosion < 7.0f)
                    {
                        _damage = 30;
                    }
                    else if (distanceFromExplosion < 15.0f)
                    {
                        _damage = 10;
                    }

                }
                else
                {
                    //Reduced damage for author of rocket to encourage rocket jumping
                    _damage = 5;
                }
                try
                {
                    _playerHit.GetComponent<MultiplayerStats>().DecreaseHealth(_damage);
                }
                catch
                {
                    //Wasn't a player
                    try
                    {
                        _playerHit.GetComponent<BotStats>().DecreaseHealth(_damage);
                    }
                    catch
                    {
                        //Wasn't a bot either!
                    }

                }


                //print(_playerHit.name + " was " + distanceFromExplosion + " from explosion and took " + _damage + " damage");
            }
        }

        //Cleanup for efficiency
        Destroy(explosionObject, 3);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Prevents parent object from causing rocket to explode. Explodes otherwise
        if (PlayerID.GetIDByGameObject(collision.gameObject) != this.GetParentID())
        {
            Explode();
        }
    }

    //Handles ID for this object
    public void SetParentByID(int sid)
    {
        this.id = sid;
    }
    public int GetParentID()
    {
        return this.id;
    }
}
