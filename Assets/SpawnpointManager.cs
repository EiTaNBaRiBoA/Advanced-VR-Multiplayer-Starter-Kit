using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointManager : MonoBehaviour
{
    public static SpawnpointManager instance;
    [HideInInspector]
    public List<Transform> spawns;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        foreach (Transform spawn in transform.GetComponentsInChildren<Transform>())
        {
            spawns.Add(spawn);
        }
    }

    public Transform GetRandomSpawn()
    {
        return spawns[Random.Range(0, spawns.Count)];
    }    
}
