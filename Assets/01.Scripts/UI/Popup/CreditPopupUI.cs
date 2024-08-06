using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditPopupUI : PopupUI
{
    public override void InitializePopup()
    {
        TimeManager.Instance.StopTimeScale();
    }
}
