using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public AudioSource sfx;

    private Dictionary<int, int> actualResolutionIndex = new Dictionary<int, int>();
    // there is a difference between the indexes of the resolutions array and the resolutions dropdown because of the refresh rates

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResIndex = 0;
        List<string> options = new List<string>();
        int maxRefreshRate = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (maxRefreshRate < resolutions[i].refreshRate) maxRefreshRate = resolutions[i].refreshRate;
        }

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (resolutions[i].refreshRate == maxRefreshRate)
            {
                options.Add(option);
                actualResolutionIndex.Add(options.Count - 1, i);
            }

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMasterVolume(float volume)
    {
        if (sfx.mute) { sfx.mute = false; }
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        sfx.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];
        if (!sfx.isPlaying && Time.time > 1) { sfx.Play(); }    //Time.time > 1 because otherwise it will play the sfx at startup.
    }

    public void SetUISFXVolume(float volume)
    {
        if (sfx.mute) { sfx.mute = false; }
        audioMixer.SetFloat("UISFXVolume", Mathf.Log10(volume) * 20);
        sfx.outputAudioMixerGroup = audioMixer.FindMatchingGroups("UI")[0];
        if (!sfx.isPlaying && Time.time > 1) { sfx.Play(); }
    }

    public void SetGameSFXVolume(float volume)
    {
        if (sfx.mute) { sfx.mute = false; }
        audioMixer.SetFloat("GameSFXVolume", Mathf.Log10(volume) * 20);
        sfx.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Game")[0];
        if (!sfx.isPlaying && Time.time > 1) { sfx.Play(); }
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[actualResolutionIndex[resolutionIndex]];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);
    }
}
