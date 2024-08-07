using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    [SerializeField] private List<ImageToggle> _playerToggles = new List<ImageToggle>();

    public void PlayerTutorialToggle(int idx)
    {
        _playerToggles[idx].ToggleSprite();
    }
}
