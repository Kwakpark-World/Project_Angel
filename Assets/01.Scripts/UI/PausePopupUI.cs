using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopupUI : PopupUI
{
    public override void InitializePopup()
    {
        Time.timeScale = 0.0f;
    }

    public override void TogglePopup(bool value)
    {
        base.TogglePopup(value);

        if (!value)
        {
            Time.timeScale = 1.0f;
        }
    }
}
