using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Mirror;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : NetworkBehaviour
{
    public float acceleration;
    public float maxVelocity;
    public float maxSlopeAngle;
    public float groundDrag;
    [Space]
    public Transform forwardTransform;

    [HideInInspector] public bool grounded;
    private RaycastHit _groundRaycast;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
            return;
        
        GroundCheck();
        _rb.useGravity = !IsOnSlope();
        _rb.drag = grounded ? groundDrag : 0;//TODO refractor
        if (grounded)
        {
            TryMove();
        }
    }

    private void TryMove()
    {
        if(_rb.velocity.magnitude > maxVelocity)
            return;
        
        //Get raw controller input
        Vector2 moveInput = Vector2.zero;
        foreach (VRHand vrHand in VRHand.allLocalHands)
        {
            Vector2 input = vrHand.GetInputScheme().Move.ReadValue<Vector2>();

            if (input.magnitude > 0)
            {
                moveInput = input;
                break;
            }
        }

        //Calculate forward and right direction of our forward object in only the xz axis (normalize as well)
        Vector3 forwardDirection = new Vector3(forwardTransform.forward.x, 0, forwardTransform.forward.z).normalized;
        Vector3 rightDirection = new Vector3(forwardTransform.right.x, 0, forwardTransform.right.z).normalized;
        
        //Map input to forward and right direction
        Vector3 mappedDirection = 
            new Vector3(rightDirection.x * moveInput.x, 0, rightDirection.z * moveInput.x) + 
            new Vector3(forwardDirection.x * moveInput.y, 0, forwardDirection.z * moveInput.y);
        
        if (IsOnSlope())
        {
            mappedDirection = SlopeProjectDirection(mappedDirection);
        }
        
        //Multiply by acceleration
        Vector3 finalAcceleration = mappedDirection * acceleration * Time.deltaTime;
        _rb.AddForce(finalAcceleration, ForceMode.Acceleration);//TODO movement works?
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, out _groundRaycast, 0.5f);
    }
    
    private bool IsOnSlope()
    {
        //https://youtu.be/xCxSjgYTw9c
        float angle = Vector3.Angle(Vector3.up, _groundRaycast.normal);
        return angle < maxSlopeAngle && angle != 0;
    }

    private Vector3 SlopeProjectDirection(Vector3 flatDirection)
    {
        return Vector3.ProjectOnPlane(flatDirection, _groundRaycast.normal);
    }
}
