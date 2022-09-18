using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideInterfacer : MonoBehaviour
{
    public float pullLength;
    public Animator animator;

    private Vector3 _startPosition;
    
    private void Start()
    {
        _startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        LockAxisMovement();
        
        animator.SetFloat("Slide Position", Mathf.Abs(
            (transform.localPosition.z - _startPosition.z) / pullLength));
    }

    private void LockAxisMovement()
    {
        //Lock Position
        Vector3 lockedPosition = _startPosition;
        lockedPosition.z = Mathf.Clamp(transform.localPosition.z, _startPosition.z - pullLength, _startPosition.z);
        transform.localPosition = lockedPosition;
        
        //Lock Rotation
        transform.localRotation = Quaternion.identity;
    }
}
