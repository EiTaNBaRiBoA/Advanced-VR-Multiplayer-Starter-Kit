using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class VRHand : MonoBehaviour
{ 
    //TODO do velocity tracking every say 0.1s (might help with throwing)
    
    public static readonly List<VRHand> allLocalHands = new List<VRHand>();
    
    public HandSide handSide;

    private Vector3 _positionLastFrame;
    private Quaternion _rotationLastFrame;
    private VRRig _vrRig;

    private void Start()
    {
        _vrRig = GetComponentInParent<VRRig>();
        
        if (_vrRig.hasAuthority)
            allLocalHands.Add(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_vrRig.hasAuthority)
            return;
        
        //Update last frame orientation before changing it
        _positionLastFrame = transform.position;
        _rotationLastFrame = transform.rotation;
        
        //Move hands according to controls
        transform.localPosition = GetInputScheme().Position.ReadValue<Vector3>();
        transform.localRotation = GetInputScheme().Rotation.ReadValue<Quaternion>();
    }
    

    public Controls GetControlSource()
    {
        return (handSide == HandSide.Left) ? _vrRig.LeftHandControlSource : _vrRig.RightHandControlSource;
    }
    
    public Controls.HandActions GetInputScheme()
    {
        return GetControlSource().Hand;
    }

    public Vector3 EstimateVelocity()
    {
        Vector3 deltaPosition = transform.position - _positionLastFrame;

        return deltaPosition / Time.deltaTime;
    }

    public Vector3 EstimateAngularVelocity()
    {
        Vector3 deltaRotation = transform.rotation.eulerAngles - _rotationLastFrame.eulerAngles;

        return deltaRotation / Time.deltaTime;
    }

    public void Vibrate(float amplitude, float duration)
    {
        XRControllerWithRumble handDevice = (XRControllerWithRumble)GetControlSource().devices.Value[0];
        
        handDevice.SendImpulse(amplitude, duration);
    }
}

public enum HandSide
{
    Right,
    Left
}