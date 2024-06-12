using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwakenSlamSkillEffect : PlayerEffect
{
    private Vector3 _startPos = Vector3.zero;
    private Vector3 _endPos = Vector3.zero;

    private Vector3 dir = Vector3.zero;
    private Quaternion rot = Quaternion.identity;
            

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();
        
        if (!GameManager.Instance.PlayerInstance.IsAwakening)
        {
            PoolManager.Instance.Push(this);
        }
        PoolManager.Instance.Push(this, duration);


        int comboCounter = gameObject.name[gameObject.name.Length - 1] - '0';


        SetAttackParams(comboCounter);


        SetRotation(comboCounter);

        PlayEffect();

        SlamAttack();
    }


    protected override void Update()
    {
        base.Update();

    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }

    private void SlamAttack()
    {
        // 공격 방향대로 오버랩
    }

    private void SetAttackParams(int comboCounter)
    {
        if (comboCounter < 2)
        {
            _startPos = transform.Find("t1").position;
            _endPos = transform.Find("t3").position;

            dir = (_endPos - _startPos).normalized;
            rot = Quaternion.LookRotation(dir);
        }
        else
        {
            dir = _player.transform.forward;
            rot = Quaternion.LookRotation(_player.transform.forward);
        }
    }

    private void SetRotation(int comboCounter)
    {
        Vector3 dir = Vector3.zero;
        if (comboCounter < 2)
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
