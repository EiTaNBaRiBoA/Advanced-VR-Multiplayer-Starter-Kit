using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Lever : NetworkBehaviour
{
    [Header("Properties")]
    public Transform stick;
    public float maxAngle;
    [Space]
    public float value;

    private Vector3 _startRotation;
    
    private void Start()
    {
        _startRotation = stick.localRotation.eulerAngles;
    }

    private void Update()
    {
        if(!isServer)
            return;
        
        float angleZ = Mathf.DeltaAngle(stick.localRotation.eulerAngles.z, _startRotation.z);
        value = (angleZ / (maxAngle * 2)) + 0.5f;
    }
}