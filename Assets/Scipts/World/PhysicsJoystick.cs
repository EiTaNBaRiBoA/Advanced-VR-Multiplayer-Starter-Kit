using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class PhysicsJoystick : NetworkBehaviour
{
    [Header("Properties")]
    public Transform stick;
    public bool resetWhenReleased;
    [Space]
    public Vector2 value;

    //TODO enable slerp drive when not held
    //TODO calculate value

    private void Update()
    {
        if (isServer)
            SetValue();

        if (!hasAuthority)
            return;

        if(resetWhenReleased && !GetComponent<XRGrabInteractable>().isSelected)
            stick.localPosition = new Vector3(0, stick.localPosition.y, 0);
    }

    private void SetValue()
    {
        value = new Vector2(stick.localPosition.x, stick.localPosition.z).normalized;
    }
}
