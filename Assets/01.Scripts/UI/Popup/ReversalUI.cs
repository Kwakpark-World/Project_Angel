using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ReversalUI : PopupUI
{
    public Camera mainCamera;
    public Canvas mainCanvas;
    public Button flipButton;
    public Button resetButton;


    public override void InitializePopup()
    {
        throw new NotImplementedException();
    }
    private bool isFlipped = false;

    void Start()
    {
        if (flipButton != null)
        {
            flipButton.onClick.AddListener(Flip);
        }
        if (resetButton != null)
        {
            resetButton.onClick.AddListener(Reset);
        }
    }

    void Flip()
    {
        isFlipped = true;
        SetScale(isFlipped);
    }

    void Reset()
    {
        isFlipped = false;
        SetScale(isFlipped);
    }

    private void SetScale(bool flipped)
    {
        float scaleY = flipped ? -1 : 1;

        if (mainCamera != null)
        {
            Vector3 cameraScale = mainCamera.transform.localScale;
            cameraScale.y = scaleY;
            mainCamera.transform.localScale = cameraScale;
        }

        if (mainCanvas != null)
        {
            Vector3 canvasScale = mainCanvas.transform.localScale;
            canvasScale.y = scaleY;
            mainCanvas.transform.localScale = canvasScale;
        }
    }
}
