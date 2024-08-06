using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageToggleButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private ImageToggleButton _oppositeButton;
    [SerializeField]
    private Color _toggleOnColor;
    [SerializeField]
    private Color _toggleOffColor;
    public UnityEvent<bool> onValueChanged;
    public bool isOn;
    private Image _toggleButtonImage;

    private void Awake()
    {
        _toggleButtonImage = GetComponent<Image>();
        _toggleButtonImage.color = isOn ? _toggleOnColor : _toggleOffColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleSprite();
        _oppositeButton.ToggleSprite();
    }

    public void ToggleSprite()
    {
        isOn = !isOn;
        _toggleButtonImage.color = isOn ? _toggleOnColor : _toggleOffColor;

        if (isOn)
        {
            onValueChanged?.Invoke(isOn);
        }
    }
}
