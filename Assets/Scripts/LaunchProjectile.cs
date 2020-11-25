using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public float projectileSpeed = 50.0f;
    public float adjustSpawnPositionY = 0.3f;
    public GameObject projectilePrefab;
    

    void Update()
    {
        if (StaticInput.GetShooting())
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        Vector3 adjustedPosition = transform.position;
        adjustedPosition.y -= adjustSpawnPositionY;

        GameObject projectile = Instantiate(projectilePrefab, adjustedPosition, transform.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
    }
}
