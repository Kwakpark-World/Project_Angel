using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    [Header("PlayerStat")]
    public Slider playerHp;
    public Slider playerAwakenGage;
    public Slider playerChargeGage;

    [Header("PlayerSkill")]
    public Image QSkill;
    public Image MouseLeftSkill;
    public Image DefenceSkill;

    private void Update()
    {
        UpdateHp();
        UpdateAwakenGage();
        UpdateChargeGage();
        
        if(!GameManager.Instance.PlayerInstance.IsAwakening)
        {
            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.NormalChargeAttack))
            {
                StartCoroutine(CoolTime(MouseLeftSkill, GameManager.Instance.PlayerInstance.PlayerStatData.chargingAttackCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.NormalSlam))
            {
                StartCoroutine(CoolTime(QSkill, GameManager.Instance.PlayerInstance.PlayerStatData.slamCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.Defense))
            {
                StartCoroutine(CoolTime(DefenceSkill, GameManager.Instance.PlayerInstance.PlayerStatData.defenseCooldown.GetValue()));
            }
        }
        else
        {
            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.AwakenChargeAttack))
            {
                StartCoroutine(CoolTime(MouseLeftSkill, GameManager.Instance.PlayerInstance.PlayerStatData.chargingAttackCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.AwakenSlam))
            {
                StartCoroutine(CoolTime(QSkill, GameManager.Instance.PlayerInstance.PlayerStatData.slamCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.Defense))
            {
                StartCoroutine(CoolTime(DefenceSkill, GameManager.Instance.PlayerInstance.PlayerStatData.defenseCooldown.GetValue()));
            }
        }

    }

    public void UpdateHp()
    {
        playerHp.value = GameManager.Instance.PlayerInstance.CurrentHealth;
    }

    public void UpdateAwakenGage()
    {
        playerAwakenGage.value = GameManager.Instance.PlayerInstance.awakenCurrentGauge;
    }

    public void UpdateChargeGage()
    {
        playerChargeGage.value = GameManager.Instance.PlayerInstance.ChargingGauge;
    }

    IEnumerator CoolTime(Image skillImage, float cooldown)
    {
        float elapsed = 0f;

        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            skillImage.fillAmount = Mathf.Clamp01(1 - (elapsed / cooldown));

            yield return null;
        }

        skillImage.fillAmount = 0f;
    }
}
