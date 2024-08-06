using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    [SerializeField]
    private Slider _xSensitivitySlider;
    [SerializeField]
    private Slider _ySensitivitySlider;
    [SerializeField]
    private TextMeshProUGUI _xSensitivityPercentageText;
    [SerializeField]
    private TextMeshProUGUI _ySensitivityPercentageText;

    [SerializeField]
    private GameObject _soundSetting;
    [SerializeField]
    private GameObject _sensitivitySetting;

    private bool _isPanelChange;

    private void Start()
    {
        foreach (var volumeSlider in _volumeSliders)
        {
            volumeSlider.Value.volumeSlider.onValueChanged.AddListener((value) => ChangeVolume(volumeSlider.Key, value));
            volumeSlider.Value.muteImageToggle.onValueChanged.AddListener((value) => ChangeVolumeSliderValue(volumeSlider.Key, value));
            ChangeVolumeSliderValue(volumeSlider.Key, 1f);
        }

        ChangeXSensitivitySliderValue();
        ChangeYSensitivitySliderValue();
    }

    public void Update()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame && !_isPanelChange)
        {
            _soundSetting.SetActive(false);
            _sensitivitySetting.SetActive(true);
            _isPanelChange = true;
        }

        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame && _isPanelChange)
        {
            _soundSetting.SetActive(true);
            _sensitivitySetting.SetActive(false);
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

    public void ChangeXSensitivitySliderValue()
    {
        GameManager.Instance.PlayerInstance.PlayerStatData.xRotateSpeed.SetDefalutValue(_xSensitivitySlider.value);
        _xSensitivityPercentageText.text = $"{_xSensitivitySlider.value.ToString("##0")}%";
    }

    public void ChangeYSensitivitySliderValue()
    {
        GameManager.Instance.PlayerInstance.PlayerStatData.yRotateSpeed.SetDefalutValue(_ySensitivitySlider.value);
        _ySensitivityPercentageText.text = $"{_ySensitivitySlider.value.ToString("##0")}%";
    }

    public void XReverse(bool value)
    {
        CameraManager.Instance.IsXReverse = value;
    }

    public void YReverse(bool value)
    {
        CameraManager.Instance.IsYReverse = value;
    }
}
