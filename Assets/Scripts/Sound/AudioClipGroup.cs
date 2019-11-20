using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioClipGroup")]
public class AudioClipGroup : ScriptableObject
{
    [Range(0, 2)]
    public float VolumeMin = 1;
    [Range(0, 2)]
    public float VolumeMax = 1;
    [Range(0, 2)]
    public float PitchMin = 1;
    [Range(0, 2)]
    public float PitchMax = 1;
    public float Cooldown = 0.1f;

    public List<AudioClip> AudioClips;

    private AudioSourcePool pool;
    private float timeStamp;

    public void OnEnable()
    {
        timeStamp = 0;
    }

    public void Play(AudioSource audioSource)
    {
        if (AudioClips == null || AudioClips.Count <= 0) return;

        if (Time.time < timeStamp) return;

        audioSource.clip = AudioClips[Random.Range(0, AudioClips.Count)];
        audioSource.volume = Random.Range(VolumeMin, VolumeMax);
        audioSource.pitch = Random.Range(PitchMin, PitchMax);
        audioSource.Play();
        timeStamp = Time.time + Cooldown;
    }

    public void Play()
    {
        if (pool == null)
        {
            pool = FindObjectOfType<AudioSourcePool>();
        }

        Play(pool.GetSource());
    }
}