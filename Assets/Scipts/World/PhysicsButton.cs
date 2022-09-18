using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    public bool pressed;
    public float threshold;
    public UnityEvent wasPressedEvent;

    private Vector3 _startPos;
    private ConfigurableJoint _joint;
    private float _lastPressed;
    
    private void Start()
    {
        _joint = GetComponentInChildren<ConfigurableJoint>();
        _startPos = _joint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pressed = GetValue() > threshold;
        
        if (pressed && Time.time - _lastPressed > 1)
        {
            wasPressedEvent.Invoke();
            
            _lastPressed = Time.time;
        }
    }

    private float GetValue()
    {
        float value = Vector3.Distance(_startPos, _joint.transform.position) / _joint.linearLimit.limit;

        return Mathf.Clamp(value, 0, 1f);
    }
}
