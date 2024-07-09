using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwakeningState : PlayerState
{
    private bool _isAwakenOn;
    private bool _isEffectOn;

    private const string _awakenEffectString = "PlayerAwakenEffect";
    private const string _awakenStartEffectString = "PlayerAwakenStartEffect";
    private ParticleSystem _thisParticle;
    ParticleSystem[] _awakenStartParticle;

    public PlayerAwakeningState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        AwakenStartEffect();

        _thisParticle = _player.effectParent.Find(_effectString).GetComponent<ParticleSystem>();
        _player.StopImmediately(true);
        _isAwakenOn = false;
        _isEffectOn = false;

        _player.awakenTime = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        _thisParticle.transform.SetParent(_player.effectParent);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_isEffectOn)
        {
            AwakenEffect();
            OnAwakening();
            _isEffectOn = true;

            if (_player.PlayerInput.isPressedKey != PressedKey.None)
            {
                if (_player.PlayerInput.isPressedKey == PressedKey.Q)
                {
                    _stateMachine.ChangeState(PlayerStateEnum.NormalSlam);
                }
                else if (_player.PlayerInput.isPressedKey == PressedKey.E)
                {
                    _stateMachine.ChangeState(PlayerStateEnum.AwakenChargeAttack);
                }

                _player.PlayerInput.isPressedKey = PressedKey.None;
                return;
            }

            if (Mathf.Abs(_player.PlayerInput.XInput) > 0.05f || Mathf.Abs(_player.PlayerInput.YInput) > 0.05f)
            {
                _stateMachine.ChangeState(PlayerStateEnum.Walk);
                return;
            }
            else
            {
                _stateMachine.ChangeState(PlayerStateEnum.Idle);
                return;
            }
            
        }
    }

    private void OnAwakening()
    {
        if (_isAwakenOn) return;

        _isAwakenOn = true;
        _player.IsAwakening = true;

        //AwakeningEffect();
        ChangeModelMaterial();
        UIManager.Instance.PlayerHUDProperty?.SetAwakenSkillIcon();

        _player.StartCoroutine(PlayerAwakening());
    }

    private void ChangeModelMaterial()
    {
        int index;
        int normalStartIndex = (int)PlayerMaterialIndex.Weapon_Normal;
        int awakenStartIndex = (int)PlayerMaterialIndex.Weapon_Awaken;

        const string weaponString = Player.weaponMatName;
        const string hairString = Player.hairMatName;
        const string armorString = Player.armorMatName;
         
        for (int i = 0; i < _player.renderers.Length; i++)
        {
            List<Material> mats = new List<Material>();
            _player.renderers[i].GetMaterials(mats);

            for (int j = 0; j < _player.renderers[i].materials.Length; j++)
            {
                index = _player.IsAwakening ? awakenStartIndex : normalStartIndex;
                string[] matName = _player.renderers[i].materials[j].name.Split(' ');

                switch (matName[0])
                {
                    case weaponString:
                        break;
                    case hairString:
                        index += 1;
                        break;
                    case armorString:
                        index += 2;
                        break;

                    default:
                        continue;
                }

                mats[j] = _player.materials[index];
            }

            _player.renderers[i].SetMaterials(mats);
        }


    }

    private void AwakenStartEffect()
    {
        _awakenStartParticle = _player.effectParent.Find(_awakenStartEffectString).GetComponentsInChildren<ParticleSystem>();

        foreach (var particle in _awakenStartParticle)
        {
            particle.Play();
        }
    }

    private void AwakeningEffect()
    {
        _thisParticle.Play();
    }

    private void AwakenEffect()
    {
        foreach (var startParticle in _awakenStartParticle)
        {
            startParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        ParticleSystem awakenParticle = _player.effectParent.Find(_awakenEffectString).GetComponent<ParticleSystem>();
        awakenParticle.transform.position = _player.transform.position;
        CameraManager.Instance.ShakeCam(0.2f, 0.3f, 3f);
        AwakeningAttack();

        awakenParticle.Play();
        _player.StartCoroutine(ResetParent(awakenParticle));
    }

    private void AwakeningAttack()
    {
        Collider[] enemies = Physics.OverlapBox(_player.transform.position, Vector3.one * 5, Quaternion.identity, _player.enemyLayer);

        foreach (var enemy in enemies)
        {
            if (enemy.TryGetComponent<Brain>(out Brain brain))
            {
                brain.OnHit(3f, false, false, 0f);
            }
        }
    }

    private IEnumerator ResetParent(ParticleSystem particles)
    {
        float time = 0;
        while (particles.main.duration >= time)
        {
            time += Time.deltaTime;
            yield return null;
        }

        particles.gameObject.transform.SetParent(_player.effectParent);
    }

    private IEnumerator PlayerAwakening()
    {
        while (_player.awakenTime < _player.PlayerStatData.GetAwakenTime())
        {
            if (_player.IsAwakening)
            {
                _player.awakenTime += Time.deltaTime;
            }
            yield return null;
        }

        while (!_player.IsGroundState) yield return null;

        if (!_player.IsDie)
        {
            EndAwakening();
        }
    }

    private void EndAwakening()
    {
        _thisParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _player.IsAwakening = false;
        ChangeModelMaterial();
        UIManager.Instance.PlayerHUDProperty?.SetNormalSkillIcon();

        if (Mathf.Abs(_player.PlayerInput.XInput) > 0.05f || Mathf.Abs(_player.PlayerInput.YInput) > 0.05f)
            _stateMachine.ChangeState(PlayerStateEnum.Walk);
        else
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }

}
