using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player Debuff Icon")]
    [SerializeField]
    private Image _poisonDebuffLeftTimeImage;
    [SerializeField]
    private Image _freezeDebuffLeftTimeImage;
    [SerializeField]
    private Image _paralysisDebuffLeftTimeImage;

    private bool _isDebuffer = false;

    [Header("Player Skill Icon")]
    [SerializeField]
    private Image _defenseSkillCooldownImage;

    [SerializeField]
    private Sprite _normalSlamSkillIcon;
    [SerializeField]
    private List<Sprite> _awakenSlamSkillIcons;
    [SerializeField]
    private Image _slamSkillIconImage;
    [SerializeField]
    private Image _slamSkillCooldownImage;

    [SerializeField]
    private Sprite _normalChargingSkillIcon;
    [SerializeField]
    private Sprite _awakenChargingSkillIcon;
    [SerializeField]
    private Image _chargingSkillIconImage;
    [SerializeField]
    private Image _chargingSkillCooldownImage;

    [Header("Player Stat")]
    [SerializeField]
    private Image _healthBarImage;
    [SerializeField]
    private TextMeshProUGUI _healthText;

    [SerializeField]
    private Slider _awakenGaugeSlider;
    [SerializeField]
    private TextMeshProUGUI _awakenGaugeText;

    [SerializeField]
    private Slider _chargingTimeSlider;
    [SerializeField]
    private TextMeshProUGUI _chargingTimeText;

    private void Update()
    {
        UpdateHealth();
        UpdateAwakenGauge();
        UpdateChargingGauge();

        if (!GameManager.Instance.PlayerInstance.IsAwakening)
        {
            SetNormalSkillIcon();
            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.NormalChargeAttack))
            {
                StartCoroutine(CooldownCoroutine(_chargingSkillCooldownImage, GameManager.Instance.PlayerInstance.PlayerStatData.chargingAttackCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.NormalSlam))
            {
                StartCoroutine(CooldownCoroutine(_slamSkillCooldownImage, GameManager.Instance.PlayerInstance.PlayerStatData.slamCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.Defense))
            {
                StartCoroutine(CooldownCoroutine(_defenseSkillCooldownImage, GameManager.Instance.PlayerInstance.PlayerStatData.defenseCooldown.GetValue()));
            }
        }
        else
        {
            SetAwakenSkillIcon();
            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.AwakenChargeAttack))
            {
                StartCoroutine(CooldownCoroutine(_chargingSkillCooldownImage, GameManager.Instance.PlayerInstance.PlayerStatData.chargingAttackCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.AwakenSlam))
            {
                StartCoroutine(CooldownCoroutine(_slamSkillCooldownImage, GameManager.Instance.PlayerInstance.PlayerStatData.slamCooldown.GetValue()));
            }

            if (GameManager.Instance.PlayerInstance.StateMachine.CompareState(PlayerStateEnum.Defense))
            {
                StartCoroutine(CooldownCoroutine(_defenseSkillCooldownImage, GameManager.Instance.PlayerInstance.PlayerStatData.defenseCooldown.GetValue()));
            }
        }
    }

    public void SetNormalSkillIcon()
    {
        _slamSkillIconImage.sprite = _normalSlamSkillIcon;
        _chargingSkillIconImage.sprite = _normalChargingSkillIcon;
    }

    public void SetAwakenSkillIcon()
    {
        _slamSkillIconImage.sprite = _awakenSlamSkillIcons[0];
        _chargingSkillIconImage.sprite = _awakenChargingSkillIcon;
    }

    public void StartCooldown(string buff)
    {
        _isDebuffer = true;
        switch (buff)
        {
            case "Poison":
                _poisonDebuffLeftTimeImage.transform.parent.gameObject.SetActive(true);
                StartCoroutine(CooldownCoroutine(_poisonDebuffLeftTimeImage, GameManager.Instance.PlayerInstance.BuffCompo.BuffStatData.poisonDuration));
                break;
            case "Freeze":
                _freezeDebuffLeftTimeImage.transform.parent.gameObject.SetActive(true);
                StartCoroutine(CooldownCoroutine(_freezeDebuffLeftTimeImage, GameManager.Instance.PlayerInstance.BuffCompo.BuffStatData.freezeDuration));
                break;
            case "Paralysis":
                _paralysisDebuffLeftTimeImage.transform.parent.gameObject.SetActive(true);
                StartCoroutine(CooldownCoroutine(_paralysisDebuffLeftTimeImage, GameManager.Instance.PlayerInstance.BuffCompo.BuffStatData.paralysisDuration));
                break;
        }
    }

    public void UpdateHealth()
    {
        float currentHealth = GameManager.Instance.PlayerInstance.CurrentHealth;
        float maxHealth = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxHealth();
        float healthRatio = currentHealth / maxHealth;
        _healthBarImage.fillAmount = healthRatio;
        _healthText.text = $"{Mathf.Clamp(currentHealth, 0f, maxHealth)} / {maxHealth}";
    }

    public void UpdateAwakenGauge()
    {
        float currentAwakenGauge = GameManager.Instance.PlayerInstance.currentAwakenGauge;
        float maxAwakenGauge = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxAwakenGauge();
        _awakenGaugeSlider.value = currentAwakenGauge;
        _awakenGaugeText.text = $"{(int)(currentAwakenGauge / maxAwakenGauge * 100f)}%";
    }

    public void UpdateChargingGauge()
    {
        float currentChargingTime = GameManager.Instance.PlayerInstance.currentChargingTime;
        float maxChargingTime = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxChargingTime();
        _chargingTimeSlider.value = currentChargingTime;
        _chargingTimeText.text = $"{(int)(currentChargingTime / maxChargingTime * 100f)}%";
    }

    private IEnumerator CooldownCoroutine(Image skillImage, float cooldown)
    {
        float elapsed = 0f;

        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            skillImage.fillAmount = Mathf.Clamp01(1 - (elapsed / cooldown));

            yield return null;
        }

        skillImage.fillAmount = 0f;

        if (_isDebuffer == true)
            skillImage.transform.parent.gameObject.SetActive(false);

    }
}
