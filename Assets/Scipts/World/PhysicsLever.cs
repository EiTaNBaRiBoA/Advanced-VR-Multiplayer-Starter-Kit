using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsLever : NetworkBehaviour
{
    [Header("Properties")]
    public Transform stick;
    public float maxAngle;
    [Space]
    public float value;
    [Header("Event")]
    public UnityEvent leverUpServer;
    public UnityEvent leverDownServer;

    private bool _lastLeverUp;
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

        bool leverUp = (value > 0.5f);
        if (leverUp != _lastLeverUp)
        {
            if(leverUp)
                leverUpServer.Invoke();
            else
                leverDownServer.Invoke();
        }
        
        _lastLeverUp = leverUp;
    }
}