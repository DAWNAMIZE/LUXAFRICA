using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;

    private static AudioManager main;
    public static AudioManager Main
    {
        get
        {
            if (main == null)
            {
                main = FindObjectOfType<AudioManager>();
            }
            return main;
        }
    }

    private void Awake()
    {
        foreach (Sounds s in sounds)
        {
            
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.pitch = s.pitch;
            s.audioSource.volume = s.volume;
            s.audioSource.loop = s.loop;

        }

        Play("BackgroundAudio");
    }

    public void Play(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Couldnt find sound " + name + " in AudioManager");
            return;
        }


        s.audioSource.Play();
       
    }

    public void Mute(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Couldnt find sound " + name + " in AudioManager");
            return;
        }


        s.audioSource.volume = 0;
    }

    public void Unmute(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("Couldnt find sound " + name + " in AudioManager");
            return;
        }


        s.audioSource.volume = s.volume;
    }
}
