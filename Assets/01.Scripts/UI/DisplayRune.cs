using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayRune : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent<DisplayRune> onClickRune;
    public RuneDataSO RuneData { get; set; }
    public Image RuneImage { get; set; }

    private void Awake()
    {
        RuneImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickRune?.Invoke(this);
    }
}
