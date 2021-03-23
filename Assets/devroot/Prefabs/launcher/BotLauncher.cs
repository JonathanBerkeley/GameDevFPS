using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotLauncher : MonoBehaviour
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

    //For stats handling, ensuring player has ammo
    private GameObject _player;
    private BotStats _playerStats;
    private bool _botShoot;

    void Start()
    {
        //Gets a reference to the block inside the launcher
        launchBlock = gameObject.GetComponent<Rigidbody>();
        launchBlock.transform.parent = transform.parent.transform;

        //Gets a reference to the player
        _player = gameObject.transform.root.gameObject;
        _playerStats = _player.GetComponent<BotStats>();

        _botShoot = false;
        canLaunch = true;
    }

    void Update()
    {
        if (_botShoot && canLaunch)
        {
            //Embedded for efficiency
            int _pAmmo = _playerStats.GetAmmo();
            if (_pAmmo > 0)
            {
                FireProjectile();

                //Sets the ammo to one less
                _playerStats.SetAmmo(--_pAmmo);
                StartCoroutine(PauseFiring());
            }

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
        _botShoot = false;
        canLaunch = true;
    }

    public void RequestShoot()
    {
        _botShoot = true;
    }
}
