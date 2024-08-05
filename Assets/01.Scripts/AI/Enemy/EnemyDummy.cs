using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummy : Brain
{
    protected override void Initialize()
    {
        base.Initialize();

        AnimatorCompo?.SetAnimationState();
        EnemyStatData.InitializeAllModifiers();

        CurrentHealth = EnemyStatData.GetMaxHealth();
        NormalAttackTimer = Time.time;
        SkillAttackTimer = Time.time;
        HealthBarCompo = PoolManager.Instance.Pop(PoolType.UI_HealthBar, transform.position + Vector3.up * 2.5f) as EnemyHealthBar;

        HealthBarCompo.SetOwner(this);
        HealthBarCompo.UpdateHealthBar();
    }

    protected override void Update()
    {

    }

    public override void OnHit(float incomingDamage, bool isHitPhysically = false, bool isCritical = false, float knockbackPower = 0)
    {
        hitEffect.RotatonEffect();

        int finalDamage = Mathf.RoundToInt(incomingDamage - EnemyStatData.GetDefensivePower());

        DamageTextCompo.SpawnParticle(enemyCenter.position, finalDamage.ToString(), Color.red, 0.5f);
        HealthBarCompo.UpdateHealthBar();


        if (GameManager.Instance.PlayerInstance.isReinforcedattack == true)
        {
            AnimatorCompo.SetAnimationState("BackAttackHit", AnimatorCompo.GetCurrentAnimationState("BackAttackHit") ? AnimationStateMode.None : AnimationStateMode.SavePreviousState);
            Debug.Log("2");
        }
        else
        {
            AnimatorCompo.SetAnimationState("Hit", AnimatorCompo.GetCurrentAnimationState("Hit") ? AnimationStateMode.None : AnimationStateMode.SavePreviousState);
            Debug.Log("12");
        }

        CameraManager.Instance.ShakeCam(0.5f, 0.3f, 0.3f);
        VolumeManager.Instance.HitMotionBlur(3, 1);
        TimeManager.Instance.TimeChange(0.85f, 1.5f);
    }
}
