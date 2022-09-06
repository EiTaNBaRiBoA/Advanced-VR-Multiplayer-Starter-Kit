using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float startVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * startVelocity, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
