using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Jonathan
//N00181859
public class SpawnHandler : MonoBehaviour
{
    [Range(0, 6)]
    public int desiredBots;
    public GameObject[] players;
    public GameObject[] spawnLocations;
    public GameObject playerPrefab;

    private List<GameObject> playersAsList;
    private int freeSpawns = 0;
    private bool DEBUG_PLAYERS = true;

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
        

        //Place all players/bots at spawn points
        foreach (GameObject player in playersAsList)
        {
            GameObject initialSpawnPoint = RandomVacantSpawn();
            player.transform.position = initialSpawnPoint.transform.position;
            player.transform.rotation = initialSpawnPoint.transform.rotation;
            
        }
    }


    //Returns gameobject to location of vacant spawn
    private GameObject RandomVacantSpawn()
    {
        //Keeps track of how many spawns are left available
        --freeSpawns;
        GameObject[] randomLocation = RandomShuffle(spawnLocations);
        return randomLocation[freeSpawns];
    }

    //Array randomizer based on popular JavaScript script for Node.JS
    //which I converted to C# below. https://github.com/Daplie/knuth-shuffle
    private GameObject[] RandomShuffle(GameObject[] ar)
    {
        int currentIndex = ar.Length;
        int randomIndex = 0;
        GameObject tmpObj;
        
        while (0 != currentIndex)
        {
            randomIndex = Random.Range(0, currentIndex);
            --currentIndex;

            //Temporary object for swapping
            tmpObj = ar[currentIndex];

            //Swapped by random index
            ar[currentIndex] = ar[randomIndex];
            ar[currentIndex] = tmpObj;
        }
        return ar;
    }
}
