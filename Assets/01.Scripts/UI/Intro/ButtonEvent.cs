using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 _originalScale;
    private bool _isEntered;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isEntered = true;

        SoundManager.Instance.PlaySFX(SFXType.UI_Button_Hover);

        transform.localScale = _originalScale * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = _originalScale;

        _isEntered = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(SFXType.UI_Button_Click);

        transform.localScale = _originalScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = _originalScale * (_isEntered ? 1.1f : 1f);
    }
}
