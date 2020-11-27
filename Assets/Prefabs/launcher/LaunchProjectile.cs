using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public float projectileSpeed = 50.0f;
    public float adjustSpawnPositionY = 0.3f;
    public float launchDelay = 1.0f;

    public GameObject launchEffect;
    public AudioClip[] soundEffects;
    public GameObject projectilePrefab;

    private Rigidbody launchBlock;
    private GameObject launchObject;
    private bool canLaunch;

    void Start()
    {
        //Gets a reference to the block inside the launcher
        launchBlock = gameObject.GetComponent<Rigidbody>();
        launchBlock.transform.parent = transform.parent.transform;

        canLaunch = true;
    }

    void Update()
    {
        if (StaticInput.GetShooting() && canLaunch)
        {
            FireProjectile();
            StartCoroutine(PauseFiring());
        }

        //Keeps the firing particle effects on the launcher regardless of speed
        if (launchObject != null)
        {
            launchObject.transform.position = launchBlock.transform.position;
        }
    }

    void FireProjectile()
    {
        //Adjust Y position of projectile launch if necessary
        Vector3 adjustedPosition = transform.position;
        adjustedPosition.y -= adjustSpawnPositionY;

        //Launch particle effect
        launchObject = (GameObject)Instantiate(launchEffect, transform.position, transform.rotation);
        
        //Plays audio for launch
        AudioSource.PlayClipAtPoint(soundEffects[0], transform.position);

        //Creates rocket at top of launcher
        GameObject projectile = Instantiate(projectilePrefab, adjustedPosition, transform.rotation);

        //Gives the rocket an ID to parent so that it doesn't collide with owner of rocket
        projectile.GetComponent<Projectile>()
            .SetParentByID(
                PlayerID.GetIDByGameObject(transform.parent.root.gameObject)
            );

        //Gives rocket momentum
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

        //Cleanup for efficiency
        Destroy(launchObject, 2);
    }

    IEnumerator PauseFiring()
    {
        canLaunch = false;
        yield return new WaitForSeconds(launchDelay);
        canLaunch = true;
    }
}
