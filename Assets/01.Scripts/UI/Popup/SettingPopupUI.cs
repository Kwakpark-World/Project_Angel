using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class VolumeSlider
{
    public Slider volumeSlider;
    public TextMeshProUGUI volumePercentageText;
    public ImageToggle muteImageToggle;
}

public class SettingPopupUI : PopupUI
{
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField, SerializedDictionary("Audio mixer group name", "Slider")]
    private SerializedDictionary<string, VolumeSlider> _volumeSliders = new SerializedDictionary<string, VolumeSlider>();

    public GameObject soundSetting;
    public GameObject sensitivitySetting;

    private bool _isPanelChange;

    private void Start()
    {
        foreach (var volumeSlider in _volumeSliders)
        {
            volumeSlider.Value.volumeSlider.onValueChanged.AddListener((value) => ChangeVolume(volumeSlider.Key, value));
            volumeSlider.Value.muteImageToggle.onValueChanged.AddListener((value) => ChangeVolumeSliderValue(volumeSlider.Key, value));
            ChangeVolumeSliderValue(volumeSlider.Key, 1f);
        }
    }

    public void Update()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && !_isPanelChange)
        {
            soundSetting.SetActive(false);
            sensitivitySetting.SetActive(true);
            _isPanelChange = true;
        }

        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame && _isPanelChange)
        {
            soundSetting.SetActive(true);
            sensitivitySetting.SetActive(false);
            _isPanelChange = false;
        }
    }

    public override void InitializePopup()
    {
        // Fill here.
    }

    public void ChangeVolume(string audioMixerGroupName, float volume)
    {
        if (!_volumeSliders[audioMixerGroupName].muteImageToggle.isOn)
        {
            volume = 0f;

            ChangeVolumeSliderValue(audioMixerGroupName, volume);
        }

        _audioMixer.SetFloat(audioMixerGroupName, volume > 0f ? Mathf.Log10(volume) * 20f : -80f);

        _volumeSliders[audioMixerGroupName].volumePercentageText.text = $"{(volume * 100f).ToString("##0")}%";
    }

    public void ChangeVolumeSliderValue(string audioMixerGroupName, float value)
    {
        _volumeSliders[audioMixerGroupName].volumeSlider.value = value;
    }

    public void ChangeVolumeSliderValue(string audioMixerGroupName, bool value)
    {
        _volumeSliders[audioMixerGroupName].volumeSlider.value = value ? 1f : 0f;

        if (value)
        {
            _volumeSliders[audioMixerGroupName].volumeSlider.onValueChanged.AddListener((value) => ChangeVolume(audioMixerGroupName, value));
        }
        else
        {
            _volumeSliders[audioMixerGroupName].volumeSlider.onValueChanged.RemoveListener((value) => ChangeVolume(audioMixerGroupName, value));
        }
    }
}
