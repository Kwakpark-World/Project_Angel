using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    [Header("PlayerStat")]
    public Image playerHp;
    public Slider playerAwakenGage;
    public Slider playerChargeGage;
    public TextMeshProUGUI HpTxt;
    public TextMeshProUGUI AwakenTxt;
    public TextMeshProUGUI ChargeTxt;

    [Header("PlayerSkill")]
    public Image QSkillCoolDown;
    public Image MouseLeftSkillCoolDown;
    public Image DefenceSkillCoolDown;
    public Image QSkill;
    public Image LBSkill;
    [Header("PlayerAwakenSkill")]
    public Image QAwkenSkill;
    public Image LBAwkenSkill;

    [Header("PlayerDebuff")]
    public Image PosisonDebuff;
    public Image FreezeDebuff;
    public Image ParalysisDebuff;

    private bool isDebuffer = false;

    private void Update()
    {
        UpdateHp();
        UpdateAwakenGage();
        UpdateChargeGage();
        
        if(!GameManager.Instance.PlayerInstance.IsAwakening)
        {
            OnNormalSkill();
            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.NormalChargeAttack))
            {
                StartCoroutine(CoolTime(MouseLeftSkillCoolDown, GameManager.Instance.PlayerInstance.PlayerStatData.chargingAttackCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.NormalSlam))
            {
                StartCoroutine(CoolTime(QSkillCoolDown, GameManager.Instance.PlayerInstance.PlayerStatData.slamCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.Defense))
            {
                StartCoroutine(CoolTime(DefenceSkillCoolDown, GameManager.Instance.PlayerInstance.PlayerStatData.defenseCooldown.GetValue()));
            }
        }
        else
        {
            OffNormalSkill();
            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.AwakenChargeAttack))
            {
                StartCoroutine(CoolTime(MouseLeftSkillCoolDown, GameManager.Instance.PlayerInstance.PlayerStatData.chargingAttackCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.AwakenSlam))
            {
                StartCoroutine(CoolTime(QSkillCoolDown, GameManager.Instance.PlayerInstance.PlayerStatData.slamCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.Defense))
            {
                StartCoroutine(CoolTime(DefenceSkillCoolDown, GameManager.Instance.PlayerInstance.PlayerStatData.defenseCooldown.GetValue()));
            }
        }
    }

    public void OnNormalSkill()
    {
        QSkill.enabled = true;
        LBSkill.enabled = true;

        if (QSkill.enabled && LBSkill.enabled)
        {
            LBAwkenSkill.enabled = false;
            QAwkenSkill.enabled = false;
        }
    }

    public void OffNormalSkill()
    {
        QSkill.enabled = false;
        LBSkill.enabled = false;

        if(!QSkill.enabled && !LBSkill.enabled)
        {
            LBAwkenSkill.enabled = true;
            QAwkenSkill.enabled = true;
        }
    }

    public void StartCoolTime(string buff)
    {
        isDebuffer = true;
        switch (buff)
        {
            case "Poison":
                PosisonDebuff.transform.parent.gameObject.SetActive(true);
                StartCoroutine(CoolTime(PosisonDebuff, GameManager.Instance.PlayerInstance.BuffCompo.BuffStatData.poisonDuration));
                break;
            case "Freeze":
                FreezeDebuff.transform.parent.gameObject.SetActive(true);
                StartCoroutine(CoolTime(FreezeDebuff, GameManager.Instance.PlayerInstance.BuffCompo.BuffStatData.freezeDuration));
                break;
            case "Paralysis":
                ParalysisDebuff.transform.parent.gameObject.SetActive(true);
                StartCoroutine(CoolTime(ParalysisDebuff, GameManager.Instance.PlayerInstance.BuffCompo.BuffStatData.paralysisDuration));
                break;
        }
    }

    public void UpdateHp()
    {
        float maxHealth = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxHealth();
        float currentHealth = GameManager.Instance.PlayerInstance.CurrentHealth;
        float healthRatio = currentHealth / maxHealth;
        playerHp.fillAmount = healthRatio;

        HpTxt.text = $"{currentHealth} / {maxHealth}";
    }

    public void UpdateAwakenGage()
    {
        playerAwakenGage.value = GameManager.Instance.PlayerInstance.awakenCurrentGauge;
        AwakenTxt.text = $"{GameManager.Instance.PlayerInstance.awakenCurrentGauge} / 100";
    }

    public void UpdateChargeGage()
    {
        playerChargeGage.value = GameManager.Instance.PlayerInstance.ChargingGauge;
        ChargeTxt.text = $"{GameManager.Instance.PlayerInstance.ChargingGauge} / 2";
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

        if(isDebuffer == true)
            skillImage.transform.parent.gameObject.SetActive(false);
        
    }
}
