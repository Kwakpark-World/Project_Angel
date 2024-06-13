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
        transform.position = _owner.transform.position + Vector3.up * 2.5f;

        AlignCamera();
    }

    public void SetOwner(Brain owner)
    {
        _owner = owner;
    }

    public void UpdateHealthBar()
    {
        _healthBarImage.fillAmount = Mathf.Clamp(_owner.CurrentHealth / _owner.EnemyStatData.GetMaxHealth(), 0f, _owner.EnemyStatData.GetMaxHealth());
    }

    private void AlignCamera()
    {
        if (Camera.main != null)
        {
            var camXform = Camera.main.transform;
            var forward = transform.position - camXform.position;

            forward.Normalize();

            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }
}
