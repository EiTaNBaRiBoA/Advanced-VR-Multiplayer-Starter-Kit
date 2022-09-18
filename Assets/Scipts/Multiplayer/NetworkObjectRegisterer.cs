using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkObjectRegisterer : MonoBehaviour
{
    private const string FolderPath = "Prefab/NetworkObjects";
    
    // Start is called before the first frame update
    void Awake()
    {
        //Register all network prefabs to manager
        GameObject[] prefabs = Resources.LoadAll<GameObject>(FolderPath + "/");
        foreach (GameObject prefab in prefabs)
            NetworkClient.RegisterPrefab(prefab);
    }
}
