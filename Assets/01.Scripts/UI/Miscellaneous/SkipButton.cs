using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    private Button _skipButton;
    private bool _isPressed = false;

    private void Awake()
    {
        _skipButton = GetComponent<Button>();

        if (_isPressed) return;
        _skipButton.onClick.AddListener(() => 
        {
            _isPressed = true;
            PoolManager.Instance.Push(FindObjectOfType<EnemyHealthBar>());
        });
        _skipButton.onClick.AddListener(() => UIManager.Instance.LoadScene("GameScene"));
    }
}
