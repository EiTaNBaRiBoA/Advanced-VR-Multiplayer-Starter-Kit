using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Sound", order = 1)]
public class Sound : ScriptableObject
{
    public AudioClip[] clips;
    
    [Space]
    [Header("Pitch")]
    [Range(.1f, 3f)] 
    public float minRandomPitch = 1;
    [Range(.1f, 3f)] 
    public float maxRandomPitch = 1;
    
    [Space]
    [Range(0f, 1f)]
    public float volume = 1;
    
    public bool loop;
    
    public static Sound[] LoadAllSounds()
    {
        return Resources.LoadAll<Sound>("Sounds/");
    }
}
