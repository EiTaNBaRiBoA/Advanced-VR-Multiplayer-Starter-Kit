using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointManager : MonoBehaviour
{
    public static SpawnpointManager instance;
    
    public Transform mainSpawn;
    public List<Transform> spawns;
    public bool randomSpawning;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void SetRandomSpawning(bool enabled)
    {
        randomSpawning = enabled;
    }
    
    public Transform GetRandomSpawn()
    {
        if(randomSpawning)
            return spawns[Random.Range(0, spawns.Count)];

        return mainSpawn;
    }    
}
