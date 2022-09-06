using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PhysicsButton : MonoBehaviour
{
    public bool pressed;
    public float threshold;

    private Vector3 _startPos;
    private ConfigurableJoint _joint;
    
    private void Start()
    {
        _joint = GetComponentInChildren<ConfigurableJoint>();
        _startPos = _joint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pressed = GetValue() > threshold;
    }

    private float GetValue()
    {
        float value = Vector3.Distance(_startPos, _joint.transform.position) / _joint.linearLimit.limit;

        return Mathf.Clamp(value, 0, 1f);
    }
}
