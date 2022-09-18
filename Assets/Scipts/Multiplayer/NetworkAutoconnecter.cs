using System.Collections;
using System.Collections.Generic;
using Mirror;
using ParrelSync;
using UnityEngine;

public class NetworkTestHUD : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager manager = GetComponent<NetworkManager>();
        bool isClone = ClonesManager.IsClone();
        
        if(isClone)
            manager.StartClient();
        else
            manager.StartHost();
    }
}
