using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown qualityDropdown;

    Resolution[] resolutions;

    void Start()
    {

        int currentQuality = QualitySettings.GetQualityLevel();


        qualityDropdown.value = currentQuality;

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

}
