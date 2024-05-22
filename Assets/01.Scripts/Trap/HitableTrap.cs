using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitableTrap : InteractableTrap
{
    protected Vector3 _playerAttackCenter;
    protected Vector3 _playerAttackHalfSize;
    protected Quaternion _playerAttackRotation;

    private bool _isHitTrap;

    protected override void Start()
    {
        base.Start();
        _isHitTrap = false;

        _playerAttackCenter = Vector3.zero;
        _playerAttackHalfSize = Vector3.zero;
        _playerAttackRotation = Quaternion.identity;

        SetPlayerAttackParameter();
    }

    protected override void Update()
    {
        base.Update();

        if (_isHitTrap && !_isPlayTrap)
        {
            OnTrap();
            _isHitTrap = false;
        }
    }

    protected override void PlayTrap()
    {
        Debug.Log("PlayTrap");
    }

    protected abstract void SetPlayerAttackParameter();
    public void HitTrap() // Player∞° »£√‚«ÿ¡‡æﬂ µ 
    {
        _isHitTrap = true;
    }

    public void EndHit() // ∞¯∞› ≥°≥µ¿ª ∂ß Player∞° »£√‚«ÿ¡‡æﬂµ 
    {
        _isHitTrap = false;
    }

    protected void GetHitableObject()
    {
        // hit
    }
}
