using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningSound : MonoBehaviour
{
    public AudioClipGroup AudioStep;

    public void PlayRunningSound()
    {
        AudioStep?.Play();
        Debug.Log("Play");
    }
}
