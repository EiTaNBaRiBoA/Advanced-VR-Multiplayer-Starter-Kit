using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DemoBowlingAlley : MonoBehaviour
{
    public PhysicsButton button;
    public Transform pinSpawn;
    public GameObject pinPrefab;
    private List<GameObject> _touchedObjects = new List<GameObject>();
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
            ClearAlley();
            SpawnPins();
            
            _lastPressed = Time.time;
        }
    }

    private void ClearAlley()
    {
        foreach (GameObject obj in _touchedObjects)
        {
            NetworkServer.Destroy(obj);
        }
    }

    private void SpawnPins()
    {
        for (int i = 0; i < pinSpawn.childCount; i++)
        {
            Transform spawn = pinSpawn.GetChild(i);
            GameObject obj = Instantiate(pinPrefab, spawn.position, spawn.rotation);
            NetworkServer.Spawn(obj);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.GetComponent<Rigidbody>() != null && col.gameObject.layer  != LayerMask.NameToLayer("Player"))
        {
            _touchedObjects.Add(col.gameObject);
        }
    }
}
