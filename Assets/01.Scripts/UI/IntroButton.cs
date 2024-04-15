using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class IntroButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(SoundEffectType.ButtonMousePoint);
        transform.localScale = originalScale * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    public void GameStart()
    {
        SceneManager.LoadScene(2); 
    }
}
