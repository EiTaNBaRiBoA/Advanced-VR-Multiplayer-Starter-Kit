
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public Sound sound;
    
    private void OnCollisionEnter(Collision other)
    {
        //TODO only on server
        SoundManager.instance.Play(sound.name, transform.position);
    }
}
