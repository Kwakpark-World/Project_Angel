using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Sprite _buttonOnSprite;
    [SerializeField]
    private Sprite _buttonOffSprite;
    public UnityEvent<bool> onValueChanged;
    public bool isOn;
    private Image _buttonSpriteSocket;

    // Reference to the Camera
    //public Camera targetCamera;

    private void Awake()
    {
        _buttonSpriteSocket = GetComponent<Image>();
        //_buttonSpriteSocket.sprite = _buttonOnSprite;
        isOn = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleSprite();
    }

    public void ToggleSprite()
    {
        _buttonSpriteSocket.sprite = isOn ? _buttonOffSprite : _buttonOnSprite;
        isOn = !isOn;

        onValueChanged?.Invoke(isOn);
    }

    public void UpAndDown(float num)
    {
        if (CameraManager.Instance != null)
        {
            if (num == 0)
            {
                // Rotate the camera upside down
                CameraManager.Instance.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if (num == 1)
            {
                // Reset the camera rotation
                CameraManager.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void LeftAndRight(float num)
    {
        if (CameraManager.Instance != null)
        {
            if (num == 0)
            {
                CameraManager.Instance.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (num == 1)
            {
                CameraManager.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

}
