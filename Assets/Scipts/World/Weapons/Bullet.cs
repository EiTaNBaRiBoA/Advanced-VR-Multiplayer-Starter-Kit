using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float startVelocity;
    public float damage;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * startVelocity, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision other)
    {
        Damageable damageable = other.gameObject.GetComponent<Damageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
        
        SoundManager.instance.Play("Bullet Hit", transform.position);
        Destroy(gameObject);
    }
}
