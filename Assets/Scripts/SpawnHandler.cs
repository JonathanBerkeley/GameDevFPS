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

    private List<GameObject> playersAsList;
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
            foreach (GameObject spawnPos in spawnLocations)
            {
                player.transform.position = spawnPos.transform.position;
                player.transform.rotation = spawnPos.transform.rotation;
            }
        }
    }

    void Update()
    {
        
    }
}
