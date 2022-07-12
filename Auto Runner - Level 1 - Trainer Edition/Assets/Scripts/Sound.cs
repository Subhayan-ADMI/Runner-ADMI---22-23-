using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string audioName;
    public GameObject associatedPrefab;

    public AudioClip audioClip;

    public bool playOnAwake;

    [Range(0f, 1f)]
    public float audioVolume;

    public bool isLooping;

    [Range(0f, 1f)]
    public float audioSpatialBlend;

    public float minimumHearingDistance;
    public float maximumHearingDistance;
    public AudioRolloffMode audio3dEffect;

    [HideInInspector]
    public AudioSource audioSource;
}
