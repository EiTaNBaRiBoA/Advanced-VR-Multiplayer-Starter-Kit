using UnityEngine.Audio;
using System;
using System.Numerics;
using Mirror;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SoundManager : NetworkBehaviour
{
    public static SoundManager instance;

    public GameObject soundPrefab;
    public Sound[] sounds;
    
    void Awake()
    {
        instance = this;
    }

    [Server]
    public void Play(string name, Vector3 position)
    {
        Sound sound = FindSound(name);
        
        float pitch = UnityEngine.Random.Range(sound.minRandomPitch, sound.maxRandomPitch);
        int soundIndex = UnityEngine.Random.Range(0, sound.clips.Length);

        instance.PlayOnClients(name, pitch, soundIndex, position);
    }
    
    [ClientRpc]
    public void PlayOnClients(string name, float pitch, int soundIndex, Vector3 position)
    {
        PlayLocal(name, pitch, soundIndex, position);
    }

    public void PlayLocal(string name, float pitch, int soundIndex, Vector3 position)
    {
        GameObject obj = Instantiate(soundPrefab);
        AudioSource source = obj.GetComponent<AudioSource>();
        
        Sound sound = FindSound(name);
        AudioClip clip = sound.clips[soundIndex];

        source.clip = clip;
        source.volume = sound.volume;
        source.pitch = pitch;
        source.loop = sound.loop;
        source.minDistance = sound.minDistance;
        source.maxDistance = sound.maxDistance;
        
        obj.transform.position = position;
        source.Play();

        if(!sound.loop)
            Destroy(obj, clip.length + 1);
    }

    public Sound FindSound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return null;
        }

        return sound;
    }
}