using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//For holding references to gameobjects after they are disabled
public class HoldReferences : MonoBehaviour
{
    public GameObject[] objects;

    public GameObject[] GetObjectReferences()
    {
        return objects;
    }

    public GameObject GetObjectReferenceByTag(string tag)
    {
        for (int i = 0; i < objects.Length; ++i)
        {
            if (objects[i].tag == tag)
            {
                return objects[i];
            }
        }
        return null;
    }

}
