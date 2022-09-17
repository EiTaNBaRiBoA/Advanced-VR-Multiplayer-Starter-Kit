using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public float minZ, maxZ;
    
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Lock axies
        transform.localPosition = new Vector3(0, 0, 
            Mathf.Clamp(transform.localPosition.z, minZ, maxZ));
        
        transform.localRotation = Quaternion.identity;
    }
}
