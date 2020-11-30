using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Basic bot wandering code
//This can get pretty crazy
public class BotController : MonoBehaviour
{
    public float offsetAmount = 3.0f;
    public int waitForNewDestination = 10;
    public float jumpForce = 400.0f;
    public BotLauncher botLauncher;

    private SpawnHandler spawnHandler;
    private Rigidbody rb;
    private bool moveAgain = true;
    private Transform _playerTransform;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        spawnHandler = GameObject.FindGameObjectWithTag("SP_SpawnHandler").GetComponent<SpawnHandler>();
        rb.AddForce(Random.Range((0 - offsetAmount), (0 + offsetAmount))
            , 0
            , Random.Range((0 - offsetAmount), (0 + offsetAmount)));

        _playerTransform = GameObject.FindObjectOfType<PlayerStats>().gameObject.transform;
    }
    private void Update()
    {
        if (moveAgain)
        {
            moveAgain = false;
            StartCoroutine(BotAI());
        }

        //Face player
        rb.gameObject.transform.LookAt(_playerTransform);
    }

    private Vector3 DestinationOffset(Vector3 sd)
    {
        //Gets a randomish destination
        return new Vector3(
            Random.Range((sd.x - offsetAmount), (sd.x + offsetAmount))
            , sd.y
            , Random.Range((sd.z - offsetAmount), (sd.z + offsetAmount))
            );
    }

    //Random actions
    IEnumerator BotAI()
    {
        rb.AddForce(Random.Range((0 - offsetAmount), (0 + offsetAmount))
            , 0
            , Random.Range((0 - offsetAmount), (0 + offsetAmount)));

        //Random jump
        if ((int)Random.Range(1, 4) == 3)
        {
            rb.AddForce(0, jumpForce, 0);
        }

        //Randomally decide to shoot
        if ((int)Random.Range(1, 4) == 3)
        {
            botLauncher.RequestShoot();
        }

        yield return new WaitForSeconds(waitForNewDestination);
        moveAgain = true;
    }
}
