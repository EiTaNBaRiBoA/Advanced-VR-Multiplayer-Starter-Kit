
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public string soundName;
    
    private void OnCollisionEnter(Collision other)
    {
        SoundManager.instance.Play(soundName, transform.position);
    }
}
