using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor;
using UnityEngine;

public class NetworkObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    
    // Update is called once per frame
    void Update()
    {
        if (!NetworkManager.singleton.isNetworkActive)
            return;

        if (NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
        {
            Destroy(gameObject);
            return;
        }
        
        GameObject obj = Instantiate(prefab, transform.parent);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        
        NetworkServer.Spawn(obj);
        Destroy(gameObject);
    }
}
