using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gets a reference to the players internal ID
public class PlayerPassport : MonoBehaviour
{
    public static int MyID;
    void Start()
    {
        MyID = PlayerID.GetIDByGameObject(gameObject);
    }
}
