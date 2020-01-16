using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatFade : MonoBehaviour
{
    public Animation animation;

    private void Start()
    {
        animation = GetComponent<Animation>();
    }

    void FadeOut()
    {
        LevelEvents.RespawnPlayer();
        animation.clip = animation.GetClip("FadeIn");
        animation.Play();
    }

    void FadeIn()
    {
        animation.clip = animation.GetClip("In");
        animation.Play();
    }
}
