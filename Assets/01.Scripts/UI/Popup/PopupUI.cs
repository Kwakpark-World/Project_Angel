using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupUI : MonoBehaviour
{
    public abstract void InitializePopup();

    public void TogglePopup(bool value)
    {
        if (gameObject.activeSelf == value)
        {
            return;
        }

        gameObject.SetActive(value);

        if (value)
        {
            InitializePopup();
        }
    }
}
