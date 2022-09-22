using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using UnityEngine;

public class NetworkAutoconnector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string hostFile = Application.dataPath + "/../.host";
        
        NetworkManager manager = GetComponent<NetworkManager>();
        bool host = Application.isEditor && File.Exists(hostFile);

        if (host)
            manager.StartHost();
        else
            manager.StartClient();
    }
}
