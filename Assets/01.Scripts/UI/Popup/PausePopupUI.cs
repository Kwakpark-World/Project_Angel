using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePopupUI : PopupUI
{
    public override void InitializePopup()
    {
        Time.timeScale = 0.0f;
    }

    public override void TogglePopup(bool value)
    {
        if (value && SceneManager.GetActiveScene().name != "GameScene")
        {
            // Debug
            if (SceneManager.GetActiveScene().name != "EnemyScene")
            {
                return;
            }

            //return;
        }

        base.TogglePopup(value);

        if (!value)
        {
            Time.timeScale = 1.0f;
        }
    }
}
