using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundPanel : MonoBehaviour
{
    public AudioMixer audioMixer; // Inspector에서 AudioMixer를 할당해야 합니다.
    [Header("Volume")]
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider; // Inspector에서 UISlider를 할당해야 합니다.
    public Slider sfxVolumeSlider;
    public Slider envVolumeSlider;

    private void Start()
    {
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        envVolumeSlider.onValueChanged.AddListener(SetENVVolume);

        float initialMasterVolume = PlayerPrefs.GetFloat("Master", 1f);
        float initialBGMVolume = PlayerPrefs.GetFloat("BGM", 1f);
        float initialEnvVolume = PlayerPrefs.GetFloat("ENV", 1f);
        float initialSFXVolume = PlayerPrefs.GetFloat("SFX", 1f);

        SetMasterVolume(initialMasterVolume);
        SetBGMVolume(initialBGMVolume);
        SetENVVolume(initialEnvVolume);
        SetSFXVolume(initialSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20); 
        PlayerPrefs.SetFloat("Master", volume); 
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20); 
        PlayerPrefs.SetFloat("BGM", volume); 
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
    }

    public void SetENVVolume(float volume)
    {
        audioMixer.SetFloat("ENV", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("ENV", volume);
    }
}
