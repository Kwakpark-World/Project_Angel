using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityPopupUI : PopupUI
{
    public Slider mouseXSlider;
    public Slider mouseYSlider;
    public TextMeshProUGUI XsenstivivityValue;
    public TextMeshProUGUI YsenstivivityValue;

    public override void InitializePopup()
    {

    }

    void Update()
    {
        float playerRotateSpeedPercent = GameManager.Instance.PlayerInstance.PlayerStatData.GetXRotateSpeed();
        float playerYRotateSpeedPercent = GameManager.Instance.PlayerInstance.PlayerStatData.GetYRotateSpeed();

        Debug.Log(playerRotateSpeedPercent);

        playerRotateSpeedPercent = (mouseXSlider.value);
        playerYRotateSpeedPercent = (mouseYSlider.value);

        Debug.Log(playerRotateSpeedPercent);

        GameManager.Instance.PlayerInstance.PlayerStatData.xRotateSpeed.SetDefalutValue(playerRotateSpeedPercent);
        GameManager.Instance.PlayerInstance.PlayerStatData.yRotateSpeed.SetDefalutValue(playerYRotateSpeedPercent);

        XsenstivivityValue.text = playerRotateSpeedPercent.ToString("F0") + "%";
        YsenstivivityValue.text = playerYRotateSpeedPercent.ToString("F0") + "%";
    }
}

