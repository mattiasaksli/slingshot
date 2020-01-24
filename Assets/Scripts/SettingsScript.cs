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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (!options.Contains(option))
            {
                options.Add(option);
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
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
