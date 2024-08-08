using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageToggle : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private Sprite _toggleOnSprite;
    [SerializeField]
    private Sprite _toggleOffSprite;
    public UnityEvent<bool> onValueChanged;
    public bool isOn;
    private Image _toggleSpriteSocket;

    private void Awake()
    {
        _toggleSpriteSocket = GetComponent<Image>();
        _toggleSpriteSocket.sprite = isOn ? _toggleOnSprite : _toggleOffSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleSprite();
    }

    public void ToggleSprite()
    {
        isOn = !isOn;
        _toggleSpriteSocket.sprite = isOn ? _toggleOnSprite : _toggleOffSprite;

        onValueChanged?.Invoke(isOn);
    }
}
