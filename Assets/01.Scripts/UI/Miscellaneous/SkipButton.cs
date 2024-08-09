using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    private Button _skipButton;

    private void Awake()
    {
        _skipButton = GetComponent<Button>();

        _skipButton.onClick.AddListener(() => 
        {
            PoolManager.Instance.Push(FindObjectOfType<EnemyHealthBar>());
            UIManager.Instance.TogglePopupUniquely("Pause");

            if (Time.timeScale == 0f)
            {
                UIManager.Instance.TogglePopupUniquely("Pause");
            }

            UIManager.Instance.LoadScene("GameScene");
            GameManager.Instance.IsGameStart = true;
        });
    }
}
