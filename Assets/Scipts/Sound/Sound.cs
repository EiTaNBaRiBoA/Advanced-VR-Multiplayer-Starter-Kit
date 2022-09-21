using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    [Range(.1f, 3f)] 
    public float minRandomPitch;
    [Range(.1f, 3f)] 
    public float maxRandomPitch;
    
    [Space]
    [Range(.1f, 25f)] 
    public float minDistance;
    [Range(.1f, 50f)] 
    public float maxDistance;
    
    [Space]
    [Range(0f, 1f)]
    public float volume;
    

    public AudioClip[] clips;

    public bool loop;
}
