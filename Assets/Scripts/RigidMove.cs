using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidMove : MonoBehaviour
{
    public float moveSpeed = 4500.0f;
    public float shiftAcceleration = 15.0f;
    public float jumpHeight = 6.0f;
    public float groundedLeniancy = 0.1f;

    public Transform orientation;

    private float originalMoveSpeed;
    private Rigidbody _playerBody;
    private CapsuleCollider _playerCollider;

    void Start()
    {
        originalMoveSpeed = moveSpeed;
        _playerBody = GetComponent<Rigidbody>();
        _playerCollider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        StaticInput.UpdatePlayerValues();
    }

    private bool IsGrounded()
    {
        //Ignores rocket layer to prevent using rocket to reset jump
        //Bit shifts the index of layer 10 to get a bit mask
        int rocketLayerMask = 1 << 10;
        //Flips the bit mask to collide with everything except 10
        rocketLayerMask = ~rocketLayerMask;

        //This will draw an invisible ray downwards, if it hits a non rocket object, the player is grounded.
        return Physics.Raycast(transform.position, Vector3.down,
            _playerCollider.bounds.extents.y + groundedLeniancy, rocketLayerMask);
    }

    private void Movement()
    {
        if (IsGrounded())
        {
            if (StaticInput.GetJumping())
            {
                //Player jump by adding vertical force, accounting for player mass.
                _playerBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }

        _playerBody.AddForce(orientation.transform.forward
            * StaticInput.GetVertical() * moveSpeed * Time.deltaTime);

        _playerBody.AddForce(orientation.transform.right
            * StaticInput.GetHorizontal() * moveSpeed * Time.deltaTime);
        
    }

    private Vector2 MagnitudeByOrientation()
    {
        float viewAngle = orientation.transform.eulerAngles.y;
        float movementAngle = Mathf.Atan2(_playerBody.velocity.x, _playerBody.velocity.z);
        
        //Convert to degrees
        movementAngle *= Mathf.Rad2Deg;

        float shortestAngleVM = Mathf.DeltaAngle(viewAngle, movementAngle);

        //Find player y magnitude relative to orientation
        float yMagnitude = _playerBody.velocity.magnitude;
        yMagnitude *= Mathf.Cos(shortestAngleVM * Mathf.Deg2Rad);

        //Find player x magnitude relative to orientation
        float xMagnitude = _playerBody.velocity.magnitude;
        xMagnitude *= Mathf.Cos((90 - shortestAngleVM) * Mathf.Deg2Rad);

        Vector2 result = new Vector2(xMagnitude, yMagnitude);
        return result;
    }

}

