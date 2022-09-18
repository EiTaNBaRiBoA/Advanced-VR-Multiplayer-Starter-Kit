using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<VRHand, Collider> handTriggerTracker = new();
    
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

    private void Update()
    {
        if(!hasAuthority)
            return;

        if (handTriggerTracker.Count > 0)
        {
            if (interactionPhysicsPhysicsMode == InteractionPhysicsMode.Kinematic)
                MoveToHoldingHand();
            if (interactionPhysicsPhysicsMode == InteractionPhysicsMode.VelocityTracking)
                MimicHandVelocity();
        }
    }

    private void MoveToHoldingHand()
    {
        //Quaternion tips: https://wirewhiz.com/quaternion-tips/
        if(_rb != null)
            _rb.isKinematic = true;
        
        VRHand firstHand = handTriggerTracker.Keys.ToList()[0];
        Transform firstColliderTransform = handTriggerTracker[firstHand].transform;
        
        Vector3 deltaPosition = firstColliderTransform.position - firstHand.transform.position;
        actingTransform.position -= deltaPosition;
        
        Quaternion colliderRotationOffset = Quaternion.Inverse(actingTransform.rotation) * firstColliderTransform.rotation;
        Quaternion handRotation = firstHand.transform.rotation;
        Quaternion adjustedHandRotation = handRotation * colliderRotationOffset;
        actingTransform.rotation = adjustedHandRotation;

        if (handTriggerTracker.Count == 2)
        {
            //Fetch objects
            VRHand secondHand = handTriggerTracker.Keys.ToList()[1];
            Transform secondColliderTransform = handTriggerTracker[secondHand].transform;
            
            //Calculate direction from first hand to second collider
            Vector3 secondColliderRelativePosition = (secondColliderTransform.transform.position - firstHand.transform.position);
            Quaternion colliderDirection = Quaternion.LookRotation(secondColliderRelativePosition);
            
            //Calculate difference (offset) between collider direction and forward (handle offset)
            Quaternion forwardDirection = Quaternion.LookRotation(actingTransform.forward);
            Quaternion colliderDirectionOffset = forwardDirection * Quaternion.Inverse(colliderDirection);
            
            //Calculate direction from first hand to second hand
            Vector3 secondHandRelativePosition = (secondHand.transform.position - firstHand.transform.position);
            Quaternion secondHandDirection = Quaternion.LookRotation(secondHandRelativePosition, firstHand.transform.up);
            Quaternion adjustedSecondHandDirection = secondHandDirection * colliderDirectionOffset;
            
            actingTransform.rotation = adjustedSecondHandDirection;
        }
    }

    private void MimicHandVelocity()
    {
        if (handTriggerTracker.Count > 0)
            Debug.LogWarning(gameObject.name + " grabbable has multiple triggers but is in " +
                             "Velocity tracking mode, using exclusively the first hand that grabs");
        
        _rb.velocity = handTriggerTracker.Keys.ToList()[0].EstimateVelocity();
    }

    public void Grab(VRHand hand, Collider triggerCollider)
    {
        handTriggerTracker.Add(hand, triggerCollider);
        ClaimAuthority();
    }
    
    public void Release(VRHand hand)
    {
        //TODO only when all hands are gone
        
        //If physics mode was kinematic, simulate a throw
        if(_rb != null && interactionPhysicsPhysicsMode == InteractionPhysicsMode.Kinematic)
        {
            _rb.isKinematic = false;
            _rb.angularVelocity = hand.EstimateAngularVelocity();
            _rb.velocity = hand.EstimateVelocity();
        }

        handTriggerTracker.Remove(hand);
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