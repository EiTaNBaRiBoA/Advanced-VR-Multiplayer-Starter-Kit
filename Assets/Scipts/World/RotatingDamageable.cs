using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class RotatingDamageable : MonoBehaviour
{
    public Transform rotator;
    public Vector3 destroyedRotation;
    
    private Damageable _damageable;
    private Vector3 _defaultRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        _damageable = GetComponent<Damageable>();
        _defaultRotation = rotator.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //Set rotation on clients based on whether damageable is destroyed
        transform.localRotation = Quaternion.Euler(
            (_damageable.health <= 0) ? destroyedRotation : _defaultRotation);
    }
}
