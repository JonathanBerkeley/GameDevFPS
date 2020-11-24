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

    InputManager inputManager = new InputManager();

    void Awake()
    {
        Debug.Log("Input Manager init - " + inputManager.DebugGetInputManagerCount());
    }

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
        inputManager.UpdatePlayerValues();
    }

    private bool IsGrounded()
    {
        //This will draw an invisible ray downwards, if it hits an object, the player is grounded.
        return Physics.Raycast(transform.position, Vector3.down,
            _playerCollider.bounds.extents.y + groundedLeniancy);
    }
    /*
    private Vector2 getViewOrientation()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
    */

    private void Movement()
    {
        if (IsGrounded())
        {
            if (inputManager.GetJumping())
            {
                //Player jump by adding vertical force, accounting for player mass.
                _playerBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }

        /*
        //Old movement solution
        float hMove = Input.GetAxis("Horizontal") * moveSpeed;
        float vMove = Input.GetAxis("Vertical") * moveSpeed;
        hMove *= Time.deltaTime;
        vMove *= Time.deltaTime;

        transform.Translate(hMove, 0, vMove);
        */
        
        //New movement, aimed to fix the movement through walls problem
        /*
        float xMagnitude;
        float yMagnitude;

        Vector2 pMags = MagnitudeByOrientation();
        //Unpack player magnitude vector
        xMagnitude = pMags.x;
        yMagnitude = pMags.y;
        */

        _playerBody.AddForce(orientation.transform.forward
            * inputManager.GetVertical() * moveSpeed * Time.deltaTime);

        _playerBody.AddForce(orientation.transform.right
            * inputManager.GetHorizontal() * moveSpeed * Time.deltaTime);
        
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

