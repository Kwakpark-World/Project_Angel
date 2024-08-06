using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePopupUI : PopupUI
{
    public override void InitializePopup()
    {
        TimeManager.Instance.StopTimeScale();
    }

    public override void TogglePopup(bool value)
    {
        if (value && !GameManager.Instance.HasPlayer)
        {
            return;
        }

        base.TogglePopup(value);
    }
}
