using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityPopupUI : PopupUI
{
    public Slider mouseSlider;
    public TextMeshProUGUI senstivivityValue;

    public override void InitializePopup()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        float PlayerRotateSpeed = GameManager.Instance.PlayerInstance.PlayerStatData.rotateSpeed.GetValue();
        PlayerRotateSpeed = mouseSlider.value;
        GameManager.Instance.PlayerInstance.PlayerStatData.rotateSpeed.SetDefalutValue(PlayerRotateSpeed);

        senstivivityValue.text = PlayerRotateSpeed.ToString("F0") + "%";
    }
}

