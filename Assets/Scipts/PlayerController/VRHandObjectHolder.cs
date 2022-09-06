using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandObjectHolder : MonoBehaviour
{
    //TODO two grabbables can be held in same hand
    
    //TODO vibration trigger enter
    //TODO Multiple hands on grabbable
    //TODO held at position
    //TODO hand presence pose
    public Grabbable heldObject;
    private float _holdButtonThreshold = 0.8f;
    
    private VRRig _vrRig;
    private VRHand _hand;

    private void Start()
    {
        _vrRig = GetComponentInParent<VRRig>();
    }

    private void Update()
    {
        if (!_vrRig.hasAuthority)
            return;
        
        if (heldObject == null)
        {
            CheckGrabbed();
            return;
        }
        
        heldObject.MoveToHoldingHand();
        CheckWasReleased();
    }

    private void CheckGrabbed()
    {
        //Check inside all triggers
        foreach (Collider trigger in handTriggers)
        {
            //Check all hands
            foreach (VRHand vrHand in VRHand.allLocalHands)
            {
                //Is hand within trigger zone?
                if (!trigger.bounds.Contains(vrHand.transform.position))
                    continue;
                _hand.Vibrate(0.3f, 0.1f);//TODO vibrate on enter
                
                //TODO make sure were not holding it already
                
                //Is button held down?
                if (_hand.GetInputScheme().Hold.ReadValue<float>() < _holdButtonThreshold)
                    continue;
                
                //Grab
                heldObject.holdingHand = _hand;
                heldObject.ClaimAuthority();
                _hand.Vibrate(0.1f, 0.1f);
            }
        }
    }
    
    private void CheckWasReleased()
    {
        if (_hand.GetInputScheme().Hold.ReadValue<float>() < _holdButtonThreshold)
        {
            heldObject.Release();
            heldObject.holdingHand = null;
            heldObject = null;
        }
    }
}
