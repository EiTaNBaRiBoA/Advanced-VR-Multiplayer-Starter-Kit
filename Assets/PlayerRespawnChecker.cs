using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class PlayerRespawnChecker : NetworkBehaviour
{
    void Start()
    {
        if(hasAuthority)
            GetComponent<Damageable>().destroyedAuthorityEvent.AddListener(Respawn);
    }

    private void Respawn()
    {
        transform.position = SpawnpointManager.instance.GetRandomSpawn().position;
        GetComponent<Damageable>().RequestSetHealth(100);
    }
}
