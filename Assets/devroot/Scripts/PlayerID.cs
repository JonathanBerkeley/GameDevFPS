using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Jonathan
//N00181859
public static class PlayerID
{
    //Static class for assigning IDs to players
    private static int pid = 0;
    
    //Custom type to store players
    public struct PlayersWithID
    {
        public int id;
        public GameObject po;
    }

    //Holds the playerobjects and ids
    private static List<PlayersWithID> playerList = new List<PlayersWithID>();
    
    public static int AssignNewID(GameObject go)
    {
        ++pid;
        PlayersWithID constructedPlayer;
        constructedPlayer.id = pid;
        constructedPlayer.po = go;
        playerList.Add(constructedPlayer);
        return pid;
    }

    /// <summary>
    /// Gets ID for object which has supplied ID
    /// </summary>
    /// <param name="rid">Requested ID</param>
    /// <returns>GameObject with supplied ID</returns>
    public static GameObject GetGameObjectByID(int rid)
    {
        //Returns early if the list is null
        if (playerList == null)
            return null;

        //Finds the object with requested id and returns it
        foreach (PlayersWithID pwi in playerList)
        {
            if (pwi.id == rid)
            {
                return pwi.po;
            }
        }

        return null;
    }

    public static int GetIDByGameObject(GameObject obj)
    {
        if (playerList == null)
            return -1;

        foreach (PlayersWithID pwi in playerList)
        {
            if (GameObject.ReferenceEquals(pwi.po, obj))
            {
                return pwi.id;
            }
        }

        return -1;
    }

    public static List<PlayersWithID> GetPlayersWithIDList()
    {
        return playerList;
    }

    //Utility functions below
    public static void ResetPlayerList()
    {
        pid = 0;
        playerList.Clear();
    }

    public static int GetCurrentID()
    {
        return pid;
    }
}
