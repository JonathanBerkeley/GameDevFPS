using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public enum RotationAxis
    {
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxis Axis = RotationAxis.MouseX;

    public float minimumVerticalLook = -90.0f;
    public float maximumVerticalLook = 90.0f;

    public float horizonitalSensitivity = 2.0f;
    public float verticalSensitivity = 2.0f;

    public bool lockCursor = true;
    public bool forceHideCursor = true;

    private float _rotationX = 0.0f;

    void Update()
    {
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.Confined;
        if (forceHideCursor)
            Cursor.visible = false;

        //For X
        if (Axis == RotationAxis.MouseX)
            transform.Rotate(0, Input.GetAxisRaw("Mouse X") * horizonitalSensitivity, 0);
        //For Y
        else if (Axis == RotationAxis.MouseY)
        {
            _rotationX -= Input.GetAxisRaw("Mouse Y") * verticalSensitivity;
            _rotationX = Mathf.Clamp(_rotationX, minimumVerticalLook, maximumVerticalLook);

            float rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
    }
}
