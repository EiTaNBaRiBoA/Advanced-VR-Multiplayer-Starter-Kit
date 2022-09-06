using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DemoBallDispenser : MonoBehaviour
{
    public PhysicsButton button;
    public Transform spawn;
    public GameObject ballPrefab;
    private float _lastPressed;
    

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
        {
            Destroy(this);
            return;
        }

        if (button.pressed && Time.time - _lastPressed > 1)
        {
            GameObject obj = Instantiate(ballPrefab, spawn.position, spawn.rotation);
            NetworkServer.Spawn(obj);
            
            _lastPressed = Time.time;
        }
    }
}
