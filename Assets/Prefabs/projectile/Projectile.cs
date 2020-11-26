using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Based on code from following this tutorial https://www.youtube.com/watch?v=BYL6JtUdEY0
public class Projectile : MonoBehaviour
{
    public float delay = 3.0f;
    public float blastRadius = 5.0f;
    public float blastForce = 2000.0f;
    public GameObject explosionEffect;
    public AudioClip explosionAudio;

    private float countdown;
    private bool exploded;

    void Start()
    {
        countdown = delay;
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
        AudioSource.PlayClipAtPoint(explosionAudio, transform.position);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        //Affect nearby objects
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(blastForce, transform.position, blastRadius);
            }
        }
        
        //Cleanup for efficiency
        Destroy(explosionObject, 3);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 9)
        {
            Explode();
        }
    }
}
