using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    [SerializeField] private List<ImageToggle> _playerToggles = new List<ImageToggle>();
    private bool _isOnEvent;

    public void PlayerTutorialToggle(int idx)
    {
        _playerToggles[idx].ToggleSprite();
    }

    private void Update()
    {
        foreach (ImageToggle toggle in _playerToggles)
        {
            if (!toggle.isOn) return;
        }

        if (!_isOnEvent)
        {
            _isOnEvent = true;

            PoolManager.Instance.Push(FindObjectOfType<EnemyHealthBar>());
            UIManager.Instance.LoadScene("GameScene");
        }
    }
}
