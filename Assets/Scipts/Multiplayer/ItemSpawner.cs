using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    public List<PhysicsButton> buttons = new List<PhysicsButton>();
    
    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.singleton.mode == NetworkManagerMode.ClientOnly)
            return;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].pressed)
            {
                Spawn(prefabs[i]);
            }
        }
    }

    public void Spawn(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(obj);
    }
}
