
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoubleXRGrabInteractable : XRGrabInteractable
{
    /* Sources:
     * https://circuitstream.com/blog/two-handed-interactions-with-unity-xr-interaction-toolkit/#z4
     * https://www.youtube.com/watch?v=Ie0-oKN3Lq0
     *
     * Quaternion Math:
     * https://wirewhiz.com/quaternion-tips/
     * 
     */
    
    public Transform secondAttachTransform;
    public bool snapToSecondGrab;
    private Quaternion _secondHandRotationOffset;

    protected override void Awake()
    {
        base.Awake();
        selectMode = InteractableSelectMode.Multiple;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(interactorsSelecting.Count == 1)
            base.ProcessInteractable(updatePhase);
        
        if(interactorsSelecting.Count == 2 && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            ProcessDoubleGrip();
        }
    }

    private void ProcessDoubleGrip()
    {
        Transform firstAttach = GetAttachTransform(null);
        Transform firstHand = interactorsSelecting[0].transform;
        Quaternion targetRotation = CalculateTwoHandRotation();

        if (!snapToSecondGrab)
            targetRotation = targetRotation * _secondHandRotationOffset;

        Vector3 worldDirectionFromHandleToBase = transform.position - firstAttach.position;
        Vector3 localDirectionFromHandleToBase = transform.InverseTransformDirection(worldDirectionFromHandleToBase);

        Vector3 targetPosition = firstHand.position + targetRotation * localDirectionFromHandleToBase;
        
        transform.SetPositionAndRotation(targetPosition, targetRotation);
    }

    private Quaternion CalculateTwoHandRotation()
    {
        Transform firstHand = interactorsSelecting[0].transform;
        Transform secondHand = interactorsSelecting[1].transform;
        
        Vector3 directionBetweenHands = secondHand.position - firstHand.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionBetweenHands, firstHand.up);

        return targetRotation;
    }

    protected override void Grab()
    {
        //Only do default behaviour for first grab
        if(interactorsSelecting.Count == 1)
            base.Grab();
        if (interactorsSelecting.Count == 2)
            _secondHandRotationOffset = Quaternion.Inverse(CalculateTwoHandRotation()) * transform.rotation;
    }

    protected override void Drop()
    {
        if(!isSelected)
            base.Drop();
    }
}
