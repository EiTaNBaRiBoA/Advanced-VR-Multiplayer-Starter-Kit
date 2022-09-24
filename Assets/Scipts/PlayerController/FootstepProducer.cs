using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class FootstepProducer : MonoBehaviour
{
    public float distancePerSound;
    
    private float _distanceTraveledSinceLast;
    private Rigidbody _rb;
    private PlayerMovement _movement;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        _distanceTraveledSinceLast += _rb.velocity.magnitude * Time.deltaTime;
        
        if (!_movement.grounded)
            return;
        if (_distanceTraveledSinceLast < distancePerSound)
            return;
        
        SoundManager.instance.Play("Footstep", transform.position);
        _distanceTraveledSinceLast = 0;
    }
}
