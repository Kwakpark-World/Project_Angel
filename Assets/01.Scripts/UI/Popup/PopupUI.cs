using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupUI : MonoBehaviour
{
    public abstract void InitializePopup();

    public void TogglePopup()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            InitializePopup();
        }
    }
}
