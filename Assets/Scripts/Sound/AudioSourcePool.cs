using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    public int NumOfAudioSources;
    public AudioSource AudioSourcePrefab;

    private List<AudioSource> audioSources;
    void Awake()
    {
        audioSources = new List<AudioSource>();
        for (int i = 0; i < NumOfAudioSources; i++)
        {
            CreateAudioSource();
        }
    }

    public AudioSource CreateAudioSource()
    {
        AudioSource source = GameObject.Instantiate(AudioSourcePrefab, this.transform);
        audioSources.Add(source);
        return source;
    }

    public AudioSource GetSource()
    // Gets the first available audiosource
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                return audioSources[i];
            }
        }
        return CreateAudioSource();
    }
}