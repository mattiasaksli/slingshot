using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "HZ";
            options.Add(option);

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
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetUISFXVolume(float volume)
    {
        audioMixer.SetFloat("UISFXVolume", volume);
    }

    public void SetGameSFXVolume(float volume)
    {
        audioMixer.SetFloat("GameSFXVolume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);
        Application.targetFrameRate = res.refreshRate;
    }
}
