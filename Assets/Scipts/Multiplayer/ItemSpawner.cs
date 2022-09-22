using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawn;
    
    public void Spawn()
    {
        if (NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
            return;
        
        GameObject obj = Instantiate(prefab, spawn.position, Quaternion.identity);
        NetworkServer.Spawn(obj);
    }
}
