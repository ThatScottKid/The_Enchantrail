using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioEvent : ScriptableObject
{
    public AudioClip[] clips;       //Take an array of audio clips
    public float volume, pitch;

    public void Play(AudioSource source)
    {
        if(clips.Length == 0) return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = volume;
        source.pitch = pitch;
        source.Play();      //...and play a random one!
    }
}
