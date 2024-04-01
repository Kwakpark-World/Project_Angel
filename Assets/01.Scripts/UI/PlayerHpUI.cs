using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    public Slider _maxHpBar;

    public Slider _maxidentityBar;
    public Slider _identityBar;

    public PlayerController _playerController;
    public Player _player;

    private void Start()
    {
        //_maxHpBar.value = _playerController.
    }

    private void Update()
    {
        PlayerHP();
        PlayerAwaken();
    }

    public void PlayerHP()
    {
        _maxHpBar.value = _playerController.CurrentHealth;
    }

    public void PlayerAwaken()
    {
        _identityBar.value = _player.awakenCurrentGage;

        if(_player.IsAwakening == true)
        {
            _identityBar.value -= Time.deltaTime;
        }
    }
}