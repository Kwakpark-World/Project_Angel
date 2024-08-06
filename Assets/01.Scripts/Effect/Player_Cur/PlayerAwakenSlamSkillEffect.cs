using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwakenSlamSkillEffect : PlayerEffect
{
    public int index;
    public override void InitializePoolItem()
    {
        base.InitializePoolItem();
        
        if (!GameManager.Instance.PlayerInstance.IsAwakened)
        {
            PoolManager.Instance.Push(this);
        }
        PoolManager.Instance.Push(this, duration);

        SetRotation(index);

        PlayEffect();
    }


    protected override void Update()
    {
        base.Update();

    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }

    private void SetRotation(int index)
    {
        Vector3 dir = Vector3.zero;
        if (index < 2)
        {
            dir.y = _player.transform.eulerAngles.y;
        }
        else
        {
            dir.x = -90f; // default Effect angle
            dir.z = _player.transform.eulerAngles.y; // x가 돌아가서 이펙트의 Y회전이 아닌 Z회전을 해줘야됨
        }
        Quaternion rot = Quaternion.Euler(dir);

        transform.rotation = rot;
    }

    private void PlayEffect()
    {
        particles = GetComponentsInChildren<ParticleSystem>();

        foreach (var particle in particles)
        {
            if (particle.TryGetComponent<BazierCurveEffect>(out var bazier))
            {
                particle.Stop();
                bazier.ResetPosition();

                particle.Play();
                bazier.StartMove();
            }
            else
                particle.Play();
        }
    }
}
