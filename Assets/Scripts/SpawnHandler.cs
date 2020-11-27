using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Jonathan
//N00181859
public class SpawnHandler : MonoBehaviour
{
    [Range(0, 7)]
    public int desiredBots;
    public GameObject[] players;
    public GameObject[] spawnLocations;
    public GameObject playerPrefab;
    public bool DEBUG_PLAYERS = false;

    private List<GameObject> playersAsList;
    private int freeSpawns = 0;

    void Awake()
    {
        if (players.Length == 0)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        playersAsList = new List<GameObject>(players);

        for (int i = 0; i < desiredBots; ++i)
        {
            playersAsList.Add(Instantiate(playerPrefab, transform.position, transform.rotation));
        }

        //Assign all the players an ID through static manager class
        foreach (GameObject player in playersAsList)
        {
            PlayerID.AssignNewID(player);
        }

        //Find all spawn locations
        if (spawnLocations.Length == 0)
        {
            spawnLocations = GameObject.FindGameObjectsWithTag("SpawnPoint");
        }
        freeSpawns = spawnLocations.Length;
    }
    void Start()
    {
        spawnLocations = GameObject.FindGameObjectsWithTag("SpawnPoint");
        
        if (DEBUG_PLAYERS)
        {
            foreach (PlayerID.PlayersWithID pwi in PlayerID.GetPlayersWithIDList())
            {
                Debug.Log("Found player with ID: "
                    + pwi.id
                    + " And gameobject name "
                    + pwi.po.name);
            }
        }

        //Shuffles array of spawn locations
        RandomShuffle(spawnLocations);
        
        //Place all players/bots at spawn points
        foreach (GameObject player in playersAsList)
        {
            GameObject initialSpawnPoint = RandomVacantSpawn(spawnLocations);
            player.transform.position = initialSpawnPoint.transform.position;
            player.transform.rotation = initialSpawnPoint.transform.rotation;
            
        }
    }


    //Returns gameobject to location of vacant spawn
    private GameObject RandomVacantSpawn(GameObject[] rl)
    {
        //Keeps track of how many spawns are left available
        --freeSpawns;
        return rl[freeSpawns];
    }

    //Array randomizer based on popular JavaScript script for Node.JS
    //which I converted to C# below. https://github.com/Daplie/knuth-shuffle
    //https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
    private void RandomShuffle(GameObject[] ar)
    {
        int currentIndex = ar.Length;

        for (int i = 0; i < currentIndex; ++i)
        {
            GameObject spawn = ar[i];
            int randomIndex = Random.Range(i, currentIndex);
            ar[i] = ar[randomIndex];
            ar[randomIndex] = spawn;
        }
    }
}
