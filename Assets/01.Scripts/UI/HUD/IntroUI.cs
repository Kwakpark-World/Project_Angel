using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;

public class IntroUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _guideText;
    [SerializeField]
    private string _nextSceneName;
    private Sequence _sequence;

    public void Start()
    {
        _sequence = DOTween.Sequence();

        _sequence.AppendInterval(1f)
            .Append(_guideText.DOFade(0f, 0.5f))
            .AppendInterval(0.1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetAutoKill(GameManager.Instance.HasPlayer);
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.press.wasPressedThisFrame)
        {
            UIManager.Instance.LoadScene(_nextSceneName);
            GameManager.Instance.SetCursor(false);

            enabled = false;
        }
    }
}
