using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwakeningState : PlayerState
{
    private bool _isAwakenOn;
    private bool _isEffectOn;

    private const string _awakenEffectString = "PlayerAwakenEffect";
    private ParticleSystem _thisParticle;

    public PlayerAwakeningState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        _thisParticle = _player.effectParent.Find(_effectString).GetComponent<ParticleSystem>();
        _player.StopImmediately(true);
        _isAwakenOn = false;
        _isEffectOn = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                AwakenEffect();
                _isEffectOn = true;
            }
        }

        if (_endTriggerCalled)
        {
            OnAwakening();

            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void OnAwakening()
    {
        if (_isAwakenOn) return;

        _isAwakenOn = true;
        _player.IsAwakening = true;

        AwakeningEffect();
        ChangeModelMaterial();
        
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

    private void AwakeningEffect()
    {
        _thisParticle.Play();
    }

    private void AwakenEffect()
    {
        ParticleSystem awakenParticle = _player.effectParent.Find(_awakenEffectString).GetComponent<ParticleSystem>();

        awakenParticle.Play();
    }

    private IEnumerator PlayerAwakening()
    {
        while (_player.awakenCurrentGauge >= 0)
        {
            if (_player.IsAwakening)
            {
                _player.awakenCurrentGauge = Mathf.Clamp(_player.awakenCurrentGauge, 0, _player.PlayerStatData.GetMaxAwakenGauge());
                _player.awakenCurrentGauge -= 10 * Time.deltaTime;
            }
            yield return null;
        }

        while (!_player.IsGroundState) yield return null;

        if (!_player.IsDie)
        {
            _thisParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _player.IsAwakening = false;
            ChangeModelMaterial();
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

   

}
