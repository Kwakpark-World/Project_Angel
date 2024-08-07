using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EnemyDummy : Brain
{
    protected override void Update()
    {

    }

    protected override void Initialize()
    {
        base.Initialize();

        AnimatorCompo?.SetAnimationState();
        EnemyStatData.InitializeAllModifiers();

        CurrentHealth = EnemyStatData.GetMaxHealth();
        NormalAttackTimer = Time.time;
        SkillAttackTimer = Time.time;
        HealthBarCompo = PoolManager.Instance.Pop(PoolType.UI_HealthBar, enemyCenter.position + Vector3.up * 0.5f) as EnemyHealthBar;

        HealthBarCompo.SetOwner(this);
        HealthBarCompo.UpdateHealthBar();
    }

    public override void OnHit(float incomingDamage, bool isHitPhysically = false, bool isCritical = false, float knockbackPower = 0)
    {
        hitEffect.RotatonEffect();

        int finalDamage = Mathf.RoundToInt(incomingDamage - EnemyStatData.GetDefensivePower());

        DamageTextCompo.SpawnParticle(enemyCenter.position, finalDamage.ToString(), Color.red, 0.5f);
        HealthBarCompo.UpdateHealthBar();

        string finalHitTrigger = (GameManager.Instance.PlayerInstance.isLastComboAttack ? "LastCombo" : "") + hitAnimationTrigger;

        AnimatorCompo.SetAnimationState(finalHitTrigger, AnimatorCompo.GetCurrentAnimationState(finalHitTrigger) ? AnimationStateMode.None : AnimationStateMode.SavePreviousState);
        CameraManager.Instance.ShakeCam(0.5f, 0.3f, 0.3f);
        TimeManager.Instance.TimeChange(0.85f, 1.5f);

        if (isHitPhysically)
        {
            StartCoroutine(Knockback(knockbackPower));
        }
    }
}
