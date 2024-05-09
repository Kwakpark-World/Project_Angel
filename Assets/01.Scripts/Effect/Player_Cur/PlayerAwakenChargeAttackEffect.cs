using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerAwakenChargeAttackEffect : PlayerEffect
{
    private const string _animName = "PlayerAttack_Charged_Awaken";
    private ParticleSystem[] _thisParticles;

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();
        float playerDuration = float.MaxValue;
        foreach (var anim in _player._playerAnims)
        {
            if (anim.name == _animName)
            {
                playerDuration = anim.length - 0.84f; 
                break;
            }
        }
        if (playerDuration == float.MaxValue)
            Debug.LogError($"Effect : {gameObject.name} of Name is not match, this Effect Duration is");

        playerDuration /= _player.AnimatorCompo.speed;

        foreach (var particle in _thisParticles)
        {
            particle.Stop();
            var main = particle.main;
            main.duration = playerDuration;
            particle.Play();
        }

        if (!GameManager.Instance.PlayerInstance.IsAwakening)
        {
            PoolManager.Instance.Push(this);
        }

        PoolManager.Instance.Push(this, duration);
    }

    protected override void Update()
    {
        base.Update();

        if (!_player.StateMachine.CompareState(_playerState))
            PoolManager.Instance.Push(this, 0.5f);
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();

        _thisParticles = GetComponentsInChildren<ParticleSystem>();
        
    }
}
