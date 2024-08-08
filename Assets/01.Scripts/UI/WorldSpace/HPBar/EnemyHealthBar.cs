using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : PoolableMono
{
    [SerializeField]
    private Image _healthBarImage;
    private Brain _owner;

    private void Update()
    {
        transform.position = _owner.enemyCenter.position + Vector3.up * 0.5f;
    }

    public void SetOwner(Brain owner)
    {
        _owner = owner;
    }

    public void UpdateHealthBar()
    {
        _healthBarImage.fillAmount = Mathf.Clamp(_owner.CurrentHealth / _owner.EnemyStatData.GetMaxHealth(), 0f, _owner.EnemyStatData.GetMaxHealth());
    }
}
