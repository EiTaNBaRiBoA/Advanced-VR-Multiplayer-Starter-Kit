using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class Grabbable : NetworkBehaviour
{
    
    [Header("Properties")]
    public InteractionPhysicsMode interactionPhysicsPhysicsMode;
    public Transform actingTransform;
    public Collider[] handTriggers;
    [Space]
    public VRHand holdingHand;
    
    private Rigidbody _rb;

    private void Start()
    {
        if (actingTransform == null)
            actingTransform = transform;
        
        //Init variables for use
        _rb = actingTransform.GetComponent<Rigidbody>();
        
        //Perform checks
        if(gameObject.layer != LayerMask.NameToLayer("Grabbable"))
            Debug.LogWarning("Grabbable object " + gameObject.name + " isn't in the Grabbable layer!");
        if(handTriggers.Length == 0)
            Debug.LogWarning("Grabbable object " + gameObject.name + " doesn't have any trigger colliders!");
    }

    public void MoveToHoldingHand()
    {
        if(!hasAuthority)
            return;

        if (interactionPhysicsPhysicsMode == InteractionPhysicsMode.Kinematic)
        {
            _rb.isKinematic = true;
            actingTransform.position = holdingHand.transform.position;
            actingTransform.rotation = holdingHand.transform.rotation;
        }
        
        if (interactionPhysicsPhysicsMode == InteractionPhysicsMode.VelocityTracking)
        {
            _rb.velocity = holdingHand.EstimateVelocity();
        }
    }

    public void Grab(VRHand hand)
    {
        holdingHand = hand;
        ClaimAuthority();
    }
    
    public void Release()
    {
        //If physics mode was kinematic, simulate a throw
        if(interactionPhysicsPhysicsMode == InteractionPhysicsMode.Kinematic)
        {
            _rb.isKinematic = false;
            _rb.angularVelocity = holdingHand.EstimateAngularVelocity();
            _rb.velocity = holdingHand.EstimateVelocity();
        }
        
        holdingHand = null;
    }
    
    [Command(requiresAuthority = false)]
    private void ClaimAuthority(NetworkConnectionToClient sender = null)
    {
        netIdentity.RemoveClientAuthority();
        netIdentity.AssignClientAuthority(sender);
    }
}

public enum InteractionPhysicsMode
{
    Kinematic,
    VelocityTracking
}