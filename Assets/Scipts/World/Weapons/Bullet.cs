using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Bullet : MonoBehaviour
{
    public float startVelocity;
    public float damage;
    public Sound hitSound;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * startVelocity, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision other)
    {
        //Get damageable component in collider or in parents
        Damageable damageable = other.gameObject.GetComponentInParent<Damageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
        
        SoundManager.instance.Play(hitSound.name, transform.position);
        Destroy(gameObject);
    }
}
