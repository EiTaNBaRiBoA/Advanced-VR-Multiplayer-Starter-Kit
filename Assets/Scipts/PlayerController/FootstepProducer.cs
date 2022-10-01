using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepProducer : MonoBehaviour
{
    public float distancePerSound;
    public Sound sound;
    
    private float _distanceTraveledSinceLast;
    private CharacterController _movement;
    
    // Start is called before the first frame update
    void Start()
    {
        _movement = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _distanceTraveledSinceLast += _movement.velocity.magnitude * Time.deltaTime;
        
        if (!_movement.isGrounded)
            return;
        if (_distanceTraveledSinceLast < distancePerSound)
            return;
        
        SoundManager.instance.Play(sound.name, transform.position);
        _distanceTraveledSinceLast = 0;
    }
}
