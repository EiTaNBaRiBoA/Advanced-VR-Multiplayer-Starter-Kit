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
    public Dictionary<VRHand, Collider> heldTriggers = new();
    
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

        if (heldTriggers.Count > 0)
        {
            if (interactionPhysicsPhysicsMode == InteractionPhysicsMode.Kinematic)
            {
                MoveToHoldingHand();
                if (heldTriggers.Count == 2)
                    AdjustRotationToSupportHand();
            }
            if (interactionPhysicsPhysicsMode == InteractionPhysicsMode.VelocityTracking)
                MimicHandVelocity();
        }
    }

    public void Grab(VRHand hand, Collider triggerCollider)
    {
        heldTriggers.Add(hand, triggerCollider);
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

        heldTriggers.Remove(hand);
    }

    public VRHand[] GetHands()
    {
        return heldTriggers.Keys.ToArray();
    }
    
    public VRHand GetHand()
    {
        VRHand[] hands = GetHands();
        if (hands.Length == 0)
            return null;
        
        return hands[0];
    }

    public VRHand GetHandForTriggerId(int triggerId)
    {
        if (triggerId >= handTriggers.Length)
            return null;

        Collider trigger = handTriggers[triggerId];
        foreach(var pair in heldTriggers)
        {
            if (pair.Value.Equals(trigger))
                return pair.Key;
        }
            
        return null;
    }
    
    [Command(requiresAuthority = false)]
    private void ClaimAuthority(NetworkConnectionToClient sender = null)
    {
        netIdentity.RemoveClientAuthority();
        netIdentity.AssignClientAuthority(sender);
    }

    private void MoveToHoldingHand()
    {
        if(_rb != null)
            _rb.isKinematic = true;
        
        VRHand firstHand = GetHands()[0];
        Transform firstColliderTransform = heldTriggers[firstHand].transform;
        
        Vector3 deltaPosition = firstColliderTransform.position - firstHand.transform.position;
        actingTransform.position -= deltaPosition;
        
        Quaternion colliderRotationOffset = Quaternion.Inverse(actingTransform.rotation) * firstColliderTransform.rotation;
        Quaternion handRotation = firstHand.transform.rotation;
        Quaternion adjustedHandRotation = handRotation * colliderRotationOffset;
        actingTransform.rotation = adjustedHandRotation;
    }

    private void AdjustRotationToSupportHand()
    {        
        //Quaternion tips: https://wirewhiz.com/quaternion-tips/

        //Fetch objects
        Transform firstHand = GetHands()[0].transform;
        VRHand secondHand = GetHands()[1];
        Transform secondColliderTransform = heldTriggers[secondHand].transform;
            
        //Calculate direction from first hand to second collider
        Vector3 secondColliderRelativePosition = (secondColliderTransform.transform.position - firstHand.position);
        Quaternion colliderDirection = Quaternion.LookRotation(secondColliderRelativePosition);
            
        //Calculate difference (offset) between collider direction and forward (handle offset)
        Quaternion forwardDirection = Quaternion.LookRotation(actingTransform.forward);
        Quaternion colliderDirectionOffset = forwardDirection * Quaternion.Inverse(colliderDirection);
            
        //Calculate direction from first hand to second hand
        Vector3 secondHandRelativePosition = (secondHand.transform.position - firstHand.position);
        Quaternion secondHandDirection = Quaternion.LookRotation(secondHandRelativePosition, firstHand.up);
        Quaternion adjustedSecondHandDirection = secondHandDirection * colliderDirectionOffset;
            
        actingTransform.rotation = adjustedSecondHandDirection;
    }

    private void MimicHandVelocity()
    {
        if (heldTriggers.Count > 0)
            Debug.LogWarning(gameObject.name + " grabbable has multiple triggers but is in " +
                             "Velocity tracking mode, using exclusively the first hand that grabs");
        
        _rb.velocity = GetHands()[0].EstimateVelocity();
    }
}

public enum InteractionPhysicsMode
{
    Kinematic,
    VelocityTracking
}