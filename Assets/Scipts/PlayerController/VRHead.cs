using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHead : MonoBehaviour
{
    private VRRig _vrRig;
    
    private void Start()
    {
        _vrRig = GetComponentInParent<VRRig>();
        
        if (!_vrRig.hasAuthority)
            DestroyOwnerComponents();
    }
    
    //Camera position has to be updated right before render for minimum delay
    private void OnPreRender()
    {
        transform.localPosition = _vrRig.HeadControlSource.Head.Position.ReadValue<Vector3>();
        transform.localRotation = _vrRig.HeadControlSource.Head.Rotation.ReadValue<Quaternion>();
    }
    

    private void DestroyOwnerComponents()
    {
        Destroy(GetComponent<Camera>());
        Destroy(GetComponent<AudioListener>());
    }
}
