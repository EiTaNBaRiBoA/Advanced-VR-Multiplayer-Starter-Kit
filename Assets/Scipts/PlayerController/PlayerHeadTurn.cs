using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Mirror;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerHeadTurn : NetworkBehaviour
{
    public float degrees;
    public float joystickThreshold;
    
    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
            return;
        
        //Get raw controller input
        float turnInput = 0;
        foreach (VRHand vrHand in VRHand.allLocalHands)
        {
            Vector2 input = vrHand.GetInputScheme().Turn.ReadValue<Vector2>();  //TODO wrong hand input

            if (input.magnitude > 0)
            {
                turnInput = input.x;
                break;
            }
        }

        if (Mathf.Abs(turnInput) < joystickThreshold)
            return;
        
        float turnDegrees = (turnInput > 0) ? degrees : -degrees;
        //TODO only do once

        TryMove(); 
    }

    private void TryMove()
    {
        /*Rigidbody rb = GetComponent<Rigidbody>();
        
        if(rb.velocity.magnitude > maxVelocity)
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
        
        //Multiply by acceleration
        Vector3 finalVelocity = mappedDirection * acceleration * Time.deltaTime;
        rb.AddForce(finalVelocity, ForceMode.Acceleration);*/
    }
}
