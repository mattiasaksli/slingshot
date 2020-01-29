using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public AudioSource sfx;
    public Slider MasterSlider;
    public Slider UISlider;
    public Slider GameSlider;
    public Slider BGMSlider;
    public Toggle FullscreenToggle;

    private Dictionary<int, int> actualResolutionIndex = new Dictionary<int, int>();
    // there is a difference between the indexes of the resolutions array and the resolutions dropdown because of the refresh rates

    private int maxRefreshRate = 0;

    private void Awake()
    {
        #region Read from prefs
        float volume = PlayerPrefs.GetFloat("MasterVolume", 0);
        audioMixer.SetFloat("MasterVolume", volume);
        MasterSlider.value = Mathf.Pow(10f, volume / 20f);

        volume = PlayerPrefs.GetFloat("UISFXVolume", 0);
        audioMixer.SetFloat("UISFXVolume", volume);
        UISlider.value = Mathf.Pow(10f, volume / 20f);

        volume = PlayerPrefs.GetFloat("GameSFXVolume", 0);
        audioMixer.SetFloat("GameSFXVolume", volume);
        GameSlider.value = Mathf.Pow(10f, volume / 20f);

        volume = PlayerPrefs.GetFloat("BGMVolume", 0);
        audioMixer.SetFloat("BGMVolume", volume);
        BGMSlider.value = Mathf.Pow(10f, volume / 20f);

        Screen.fullScreen = PlayerPrefs.GetInt("isFullscreen", 0) == 1;
        FullscreenToggle.isOn = Screen.fullScreen;

        int currentWidth = PlayerPrefs.GetInt("ResolutionW", 800);
        int currentHeight = PlayerPrefs.GetInt("ResolutionH", 600);
        int currentRefreshRate = PlayerPrefs.GetInt("ResolutionR", 59);
        Screen.SetResolution(currentWidth, currentHeight, Screen.fullScreen, currentRefreshRate);

        int currentResIndex = PlayerPrefs.GetInt("ResolutionDropdownIndex", -1);
        #endregion

        QualitySettings.vSyncCount = 0;
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (maxRefreshRate < resolutions[i].refreshRate) maxRefreshRate = resolutions[i].refreshRate;
        }

        int max_i = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (resolutions[i].refreshRate == maxRefreshRate)
            {
                if (!option.Equals("1920 x 1080"))
                {
                    options.Add(option);
                    actualResolutionIndex.Add(options.Count - 1, i);
                }
            }

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && currentResIndex == -1)
            {
                currentResIndex = i;
            }

            max_i = i;
        }

        if (!options.Contains("1920 x 1080"))
        {
            options.Add("1920 x 1080");
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
        if (!sfx.isPlaying && Time.timeSinceLevelLoad > 1) { sfx.Play(); }
        //Time.timeSinceLevelLoad > 1 because otherwise it will play the sfx at startup.
    }

    public void SetUISFXVolume(float volume)
    {
        if (sfx.mute) { sfx.mute = false; }
        audioMixer.SetFloat("UISFXVolume", Mathf.Log10(volume) * 20);
        sfx.outputAudioMixerGroup = audioMixer.FindMatchingGroups("UI")[0];
        if (!sfx.isPlaying && Time.timeSinceLevelLoad > 1) { sfx.Play(); }
    }

    public void SetGameSFXVolume(float volume)
    {
        if (sfx.mute) { sfx.mute = false; }
        audioMixer.SetFloat("GameSFXVolume", Mathf.Log10(volume) * 20);
        sfx.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Game")[0];
        if (!sfx.isPlaying && Time.timeSinceLevelLoad > 1) { sfx.Play(); }
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
        Resolution res = new Resolution();
        if (resolutionIndex == actualResolutionIndex.Count)
        {
            res = new Resolution();
            res.width = 1920;
            res.height = 1080;
            res.refreshRate = maxRefreshRate;

        }
        else
        {
            res = resolutions[actualResolutionIndex[resolutionIndex]];
        }
        Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);

        PlayerPrefs.SetInt("ResolutionW", res.width);
        PlayerPrefs.SetInt("ResolutionH", res.height);
        PlayerPrefs.SetInt("ResolutionR", res.refreshRate);
        PlayerPrefs.SetInt("ResolutionDropdownIndex", resolutionIndex);
    }

    private void OnDisable()
    {
        audioMixer.GetFloat("MasterVolume", out float masterVolume);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);

        audioMixer.GetFloat("UISFXVolume", out float uisfxVolume);
        PlayerPrefs.SetFloat("UISFXVolume", uisfxVolume);

        audioMixer.GetFloat("GameSFXVolume", out float gameVolume);
        PlayerPrefs.SetFloat("GameSFXVolume", gameVolume);

        audioMixer.GetFloat("BGMVolume", out float bgmVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);

        int fullscreen = Screen.fullScreen ? 1 : 0;
        PlayerPrefs.SetInt("isFullscreen", fullscreen);
    }
}
