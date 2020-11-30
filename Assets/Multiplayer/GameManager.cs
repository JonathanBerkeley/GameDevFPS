using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ProjectileManager> projectiles = new Dictionary<int, ProjectileManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject projectilePrefab;
    public GameObject gameLogic;

    private SpawnHandler spawnHandler;

    private void Awake()
    {
        //Make this a singleton class
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already in existence, destroying self");
            Destroy(this);
        }
    }

    private void Start()
    {
        spawnHandler = gameLogic.GetComponent<SpawnHandler>();
    }


    //This is used to handle all code related to spawning players
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        GameObject[] rl = spawnHandler.GetRandomisedSpawns();
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab
                , spawnHandler.GetRandomSpawn(rl).transform.position
                , _rotation);
            PlayerID.AssignNewID(_player);
        } else
        {
            _player = Instantiate(playerPrefab
                , spawnHandler.GetRandomSpawn(rl).transform.position
                , _rotation);
            PlayerID.AssignNewID(_player);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void SpawnProjectile(int _id, Vector3 _position, Quaternion _rotation)
    {

    }

    //Resets player list on disconnect
    public void ResetDictionary()
    {
        players = new Dictionary<int, PlayerManager>();
    }

    /*
    public void CreateProjectile(int _id, Vector3 _location, Quaternion _rotation)
    {

        GameObject projectile = Instantiate(projectilePrefab, _location, _rotation);

        //Gives rocket momentum
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 50.0f, ForceMode.VelocityChange);
    }
    */
}
