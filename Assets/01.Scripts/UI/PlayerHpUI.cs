using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    public Slider _HpBar;
    public Slider _identityBar;
    public PlayerController _playerController;
    public Player _player;

    private void Update()
    {
        PlayerHP();
        _identityBar.value = _player.awakenCurrentGauge;
    }

    public void PlayerHP()
    {
        _HpBar.value = _playerController.CurrentHealth;
    }
}
