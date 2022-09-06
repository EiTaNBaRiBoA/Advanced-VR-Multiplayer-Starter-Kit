using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using InputDevice = UnityEngine.InputSystem.InputDevice;

public class VRRig : NetworkBehaviour
{
    //TODO hanging lamps
    //TODO trackers
    //TODO guns
    //TODO light sabers
    //TODO joysticks
    //TODO noerico

    public Transform cameraOffset;
    public float triggerCameraRecenterDistance;
    public Controls HeadControlSource;
    public Controls LeftHandControlSource;
    public Controls RightHandControlSource;
    
    // Update is called once per frame
    void Update()
    {
        if (HeadControlSource == null || HeadControlSource.devices.Value.Count == 0)
            PairHMD();
        if (RightHandControlSource == null || RightHandControlSource.devices.Value.Count == 0)
            PairRightHand();
        if (LeftHandControlSource == null || LeftHandControlSource.devices.Value.Count == 0)
            PairLeftHand();

        if (IsCameraUncentered())
            RecenterCamera();
    }

    public bool IsCameraUncentered()
    {
        Vector3 deltaPosition = GetComponentInChildren<Camera>().transform.position - transform.position;
        deltaPosition.y = 0;    //Dont account for y distance
        
        return (deltaPosition.magnitude > triggerCameraRecenterDistance);
    }

    [ContextMenu("Recenter")]
    public void RecenterCamera()
    {
        Vector3 deltaPosition = GetComponentInChildren<Camera>().transform.position - transform.position;
        deltaPosition.y = 0;    //Dont recenter height
        
        transform.position += deltaPosition;
        cameraOffset.position -= deltaPosition;
    }
    
    private void PairHMD()
    {
        XRHMD headMountDevice = InputSystem.GetDevice<XRHMD>();

        if (headMountDevice == null)
            return;
            
        HeadControlSource = new Controls();
        HeadControlSource.bindingMask = InputBinding.MaskByGroup("Generic XR Controller");
        HeadControlSource.devices = new InputDevice[] {headMountDevice};
        HeadControlSource.Enable();

        Debug.Log("Paired " + headMountDevice.displayName);
    }
    
    private void PairRightHand()
    {
        XRController handDevice = XRController.rightHand;

        if (handDevice == null)
            return;
            
        RightHandControlSource = new Controls();
        RightHandControlSource.bindingMask = InputBinding.MaskByGroup("Generic XR Controller");
        RightHandControlSource.devices = new InputDevice[] {handDevice};
        RightHandControlSource.Enable();

        Debug.Log("Paired " + handDevice.displayName);
    }
    
    private void PairLeftHand()
    {
        XRController handDevice = XRController.leftHand;

        if (handDevice == null)
            return;
            
        LeftHandControlSource = new Controls();
        LeftHandControlSource.bindingMask = InputBinding.MaskByGroup("Generic XR Controller");
        LeftHandControlSource.devices = new InputDevice[] {handDevice};
        LeftHandControlSource.Enable();

        Debug.Log("Paired " + handDevice.displayName);
    }
}
