using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPopupUI : PopupUI
{
    public override void InitializePopup()
    {
        TimeManager.Instance.StopTimeScale();
    }
}
