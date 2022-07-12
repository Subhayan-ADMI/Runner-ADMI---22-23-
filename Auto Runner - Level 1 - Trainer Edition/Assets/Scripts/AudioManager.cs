using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] gameAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }

        InitializeGameAudio();

        PlaySound("TitleBG");
    }

    void InitializeGameAudio()
    {
        foreach (Sound s in gameAudio)
        {
            s.audioSource = s.associatedPrefab.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.volume = s.audioVolume;
            s.audioSource.loop = s.isLooping;
            s.audioSource.playOnAwake = s.playOnAwake;

            s.audioSource.spatialBlend = s.audioSpatialBlend;
            s.audioSource.rolloffMode = s.audio3dEffect;
            s.audioSource.minDistance = s.minimumHearingDistance;
            s.audioSource.maxDistance = s.maximumHearingDistance;
        }
    }

    public void PlaySound(string audioName)
    {
        Sound s = Array.Find(gameAudio, gameAudio => gameAudio.audioName == audioName);
        
        if (!s.audioSource.isPlaying)
        {
            s.audioSource.Play();
        }
        
    }

    public void StopSound(string audioName)
    {
        Sound s = Array.Find(gameAudio, gameAudio => gameAudio.audioName == audioName);
        
        if (s.audioSource.isPlaying)
        {
            s.audioSource.Stop();
        }
    }
}
