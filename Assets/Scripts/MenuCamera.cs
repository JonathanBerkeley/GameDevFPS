using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public GameObject focusTarget;
    public float rotateSpeed = 5.0f;

    void Update()
    {
        transform.RotateAround(focusTarget.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
