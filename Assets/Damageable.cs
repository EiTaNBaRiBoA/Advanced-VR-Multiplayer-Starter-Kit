using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : NetworkBehaviour
{
    [SyncVar]
    public float health;
    
    public UnityEvent takeDamageAuthorityEvent;
    public UnityEvent destroyedAuthorityEvent;

    [Server]
    public void TakeDamage(float damage)
    {
        health -= damage;
        takeDamageAuthorityEvent.Invoke();
        
        if(health <= 0)
            destroyedAuthorityEvent.Invoke();
    }
    
    [Command]
    public void RequestSetHealth(float newHeath)
    {
        health = newHeath;
    }
}
