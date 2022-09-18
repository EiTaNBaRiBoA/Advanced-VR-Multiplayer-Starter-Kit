using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DemoBowlingAlley : NetworkBehaviour
{
    public Transform ballSpawn;
    public GameObject ballPrefab;
    
    public Transform pinSpawn;
    public GameObject pinPrefab;
    
    private List<GameObject> _trackedPins = new List<GameObject>();
    
    public void ResetAlley()
    {
        if (!isServer)
            return;
        
        foreach (GameObject obj in _trackedPins)
        {
            NetworkServer.Destroy(obj);
        }
        
        for (int i = 0; i < pinSpawn.childCount; i++)
        {
            Transform spawn = pinSpawn.GetChild(i);
            GameObject obj = Instantiate(pinPrefab, spawn.position, spawn.rotation);
            _trackedPins.Add(obj);
            NetworkServer.Spawn(obj);
        }
    }
    
    public void SpawnBall()
    {
        if (!isServer)
            return;
        
        GameObject obj = Instantiate(ballPrefab, ballSpawn.position, Quaternion.identity);
        NetworkServer.Spawn(obj);
    }
}
