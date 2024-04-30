using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneDisplay : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityEvent<RuneDisplay> onClickRune;
    public RuneDataSO RuneData { get; set; }
    public Image RuneImage { get; set; }
    private bool _isWaiting;
    private Canvas _runeCanvas;
    private Image _waitImage;

    private void Awake()
    {
        RuneImage = GetComponent<Image>();
        _runeCanvas = GetComponent<Canvas>();
        _waitImage = transform.GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        if (_isWaiting)
        {
            _waitImage.fillAmount = (Time.time - RuneManager.Instance.UnequipWaitTimer) / RuneManager.Instance.UnequipWaitTime;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickRune?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RuneImage.raycastTarget = false;
        _runeCanvas.sortingOrder++;
        transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        if (!_isWaiting && !eventData.pointerCurrentRaycast.gameObject)
        {
            _isWaiting = true;
            RuneManager.Instance.UnequipWaitTimer = Time.time;
        }
        else if (_isWaiting && eventData.pointerCurrentRaycast.gameObject)
        {
            _isWaiting = false;
            _waitImage.fillAmount = 0f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        _runeCanvas.sortingOrder--;
        RuneImage.raycastTarget = true;

        if (_isWaiting)
        {
            if (Time.time >= RuneManager.Instance.UnequipWaitTimer + RuneManager.Instance.UnequipWaitTime)
            {
                RuneManager.Instance.TryUnequipRune(RuneManager.Instance.GetEquipedRuneIndex(RuneData));
            }

            _isWaiting = false;
            _waitImage.fillAmount = 0f;
        }
    }
}
