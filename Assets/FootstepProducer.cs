using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepProducer : MonoBehaviour
{
    public float distancePerSound;
    
    private float _distanceTraveledSinceLast;
    private Rigidbody _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _distanceTraveledSinceLast += _rb.velocity.magnitude * Time.deltaTime;

        if (_distanceTraveledSinceLast > distancePerSound)
        {
            SoundManager.instance.Play("Footstep", transform.position);
            _distanceTraveledSinceLast = 0;
        }
    }
}
