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
    private Sound[] sounds;
    
    void Awake()
    {
        instance = this;
        sounds = Sound.LoadAllSounds();
    }

    [Server]
    public void Play(string soundName, Vector3 position)
    {
        Sound sound = FindSound(soundName);
        
        float pitch = UnityEngine.Random.Range(sound.minRandomPitch, sound.maxRandomPitch);
        int soundIndex = UnityEngine.Random.Range(0, sound.clips.Length);

        instance.PlayOnClients(soundName, pitch, soundIndex, position);
    }
    
    [ClientRpc]
    public void PlayOnClients(string soundName, float pitch, int soundIndex, Vector3 position)
    {
        PlayLocal(soundName, pitch, soundIndex, position);
    }

    public void PlayLocal(string soundName, float pitch, int soundIndex, Vector3 position)
    {
        GameObject obj = Instantiate(soundPrefab);
        AudioSource source = obj.GetComponent<AudioSource>();
        
        Sound sound = FindSound(soundName);
        AudioClip clip = sound.clips[soundIndex];

        source.clip = clip;
        source.volume = sound.volume;
        source.pitch = pitch;
        source.loop = sound.loop;
        
        obj.transform.position = position;
        source.Play();

        if(!sound.loop)
            Destroy(obj, clip.length + 1);
    }

    public Sound FindSound(string soundName)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found");
            return null;
        }

        return sound;
    }
}