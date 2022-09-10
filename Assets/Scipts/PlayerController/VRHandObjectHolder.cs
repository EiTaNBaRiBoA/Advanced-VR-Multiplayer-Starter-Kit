using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VRHandObjectHolder : MonoBehaviour
{
    //TODO Multiple hands on grabbable
    //TODO Multiple grabbables on same object
    //TODO different grab buttons
    //TODO held at position
    //TODO hand presence pose
    private const float TriggerSize = 0.1f;
    private const float HoldButtonThreshold = 0.1f;
    
    public LayerMask layers;
    [HideInInspector]public Grabbable heldObject;
    
    private VRRig _vrRig;
    private VRHand _hand;
    private Grabbable _lastFrameSelectedObject;

    private void Start()
    {
        _vrRig = GetComponentInParent<VRRig>();
        _hand = GetComponent<VRHand>();
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
        Collider closestTrigger = ClosestTrigger();

        if (closestTrigger == null)
        {
            _lastFrameSelectedObject = null;
            return;
        }
        
        //Fetch all grabbables in parent
        Grabbable[] grabbables = closestTrigger.gameObject.GetComponentsInParent<Grabbable>();
        //Fetch which grabbable component trigger was assigned to
        Grabbable selectedObject = null;
        foreach (var grabbable in grabbables)
        {
            if(grabbable.handTriggers.Contains(closestTrigger));
            {
                selectedObject = grabbable;
                break;
            }
        }

        //If no grabbable component was found, object is improperly configured
        if (selectedObject == null)
        {
            Debug.LogError("Object with Grabbable Trigger had not been assigned in any Grabbable component");
            return;
        }
        
        //Check if just was Selected
        if(selectedObject != _lastFrameSelectedObject)
            _hand.Vibrate(0.3f, 0.1f);  //Vibrate
        _lastFrameSelectedObject = selectedObject;
        
        //Is button held down?
        if (_hand.GetInputScheme().Hold.ReadValue<float>() < HoldButtonThreshold)
            return;
        
        //Grab
        heldObject = selectedObject;
        heldObject.Grab(_hand);
        _hand.Vibrate(0.1f, 0.1f);
    }

    private void CheckWasReleased()
    {
        if (_hand.GetInputScheme().Hold.ReadValue<float>() < HoldButtonThreshold)
        {
            heldObject.Release(_hand);
            heldObject = null;
        }
    }

    private Collider ClosestTrigger()
    {
        Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(TriggerSize, TriggerSize, TriggerSize),
            Quaternion.identity, layers);

        //Get closest trigger
        Collider closestTrigger = null;
        float closestDistance = float.MaxValue;
        foreach (Collider trigger in cols)
        {
            float distance = Vector3.Distance(transform.position, trigger.gameObject.transform.position);
            if (distance < closestDistance)
            {
                closestTrigger = trigger;
                closestDistance = distance;
            }
        }

        return closestTrigger;
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(transform.position, new Vector3(TriggerSize, TriggerSize, TriggerSize));
    }

}
