using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[Serializable]
public class Sensitivity
{
    public Slider Slider;
    public TextMeshProUGUI PercentText;
    public ImageToggle Image;
    private float _value;

    public float Value
    {
        get { return _value; }
        set
        {
            _value = value;
            Slider.value = _value;
            PercentText.text = $"{(_value * 100f).ToString("##0")}%";
        }
    }

    public void BindSliderValueChanged(UnityEngine.Events.UnityAction<float> action)
    {
        Slider.onValueChanged.AddListener(action);
    }

    public void UnbindSliderValueChanged(UnityEngine.Events.UnityAction<float> action)
    {
        Slider.onValueChanged.RemoveListener(action);
    }

    public void BindImageValueChanged(UnityEngine.Events.UnityAction<bool> action)
    {
        Image.onValueChanged.AddListener(action);
    }

    public void UnbindImageValueChanged(UnityEngine.Events.UnityAction<bool> action)
    {
        Image.onValueChanged.RemoveListener(action);
    }
}

public class MouseSensitivityPopupUI : PopupUI
{
    [SerializeField, SerializedDictionary("Sensitivity name", "Sensitivity")]
    private SerializedDictionary<string, Sensitivity> _sensitivities = new SerializedDictionary<string, Sensitivity>();

    private void Start()
    {
        foreach (var sensitivity in _sensitivities.Values)
        {
            
            sensitivity.BindSliderValueChanged((value) => UpdateSensitivity(sensitivity, value));
            sensitivity.BindImageValueChanged((value) => UpdateSensitivitySliderValue(sensitivity, value));
        }
    }

    public override void InitializePopup()
    {
        foreach (var sensitivity in _sensitivities.Values)
        {
            sensitivity.Value = 0f;
        }
    }

    private void UpdateSensitivity(Sensitivity sensitivity, float value)
    {
        sensitivity.Value = value;
    }

    private void UpdateSensitivitySliderValue(Sensitivity sensitivity, bool value)
    {
        GameManager.Instance.PlayerInstance.PlayerStatData.GetRotateSpeed();

        if (value)
        {
            sensitivity.Slider.value = sensitivity.Value;
            sensitivity.BindSliderValueChanged((value) => UpdateSensitivity(sensitivity, value));
            
        }
        else
        {
            sensitivity.Slider.value = 0f;
            sensitivity.UnbindSliderValueChanged((value) => UpdateSensitivity(sensitivity, value));
        }
    }
}

