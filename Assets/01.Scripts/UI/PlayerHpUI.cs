using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    public Slider _maxHpBar;
    public Slider _identityBar;

    public PlayerController _playerController;
    
    private void Update()
    {
        _maxHpBar.value = _playerController.CurrentHealth;
    }
}