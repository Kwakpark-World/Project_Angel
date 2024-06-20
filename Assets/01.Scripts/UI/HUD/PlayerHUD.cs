using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player Buff Icon")]
    [SerializeField]
    private Image _poisonBuffDurationImage;
    [SerializeField]
    private Image _freezeBuffDurationImage;
    [SerializeField]
    private Image _paralysisBuffDurationImage;

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

    private Player _player;
    private Dictionary<PlayerStateEnum, Coroutine> _cooldownCoroutines = new Dictionary<PlayerStateEnum, Coroutine>();

    private void Start()
    {
        if (GameManager.Instance.HasPlayer)
        {
            _player = GameManager.Instance.PlayerInstance;
        }

        foreach (PlayerStateEnum playerState in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            _cooldownCoroutines[playerState] = null;
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

    public void UpdateSkillComboIcon(int comboCounter)
    {
        _slamSkillIconImage.sprite = _awakenSlamSkillIcons[comboCounter % 3];
    }

    public void StartBuffDuration(BuffType buffType, float duration = 0)
    {
        switch (buffType)
        {
            case BuffType.Potion_Poison:
                _poisonBuffDurationImage.transform.parent.parent.parent.gameObject.SetActive(true);
                StartCoroutine(BuffDurationCoroutine(buffType, _poisonBuffDurationImage, duration));

                break;

            case BuffType.Potion_Freeze:
                _freezeBuffDurationImage.transform.parent.parent.parent.gameObject.SetActive(true);
                StartCoroutine(BuffDurationCoroutine(buffType, _freezeBuffDurationImage, duration));

                break;

            case BuffType.Potion_Paralysis:
                _paralysisBuffDurationImage.transform.parent.parent.parent.gameObject.SetActive(true);
                StartCoroutine(BuffDurationCoroutine(buffType, _paralysisBuffDurationImage, duration));

                break;

            default:
                break;
        }
    }

    public void StartSkillCooldown(PlayerStateEnum playerState)
    {
        if (_cooldownCoroutines[playerState] != null)
        {
            return;
        }

        switch (playerState)
        {
            case PlayerStateEnum.Defense:
                _cooldownCoroutines[playerState] = StartCoroutine(SkillCooldownCoroutine(playerState, _defenseSkillCooldownImage, GameManager.Instance.PlayerInstance.PlayerStatData.GetDefenseCooldown()));

                break;

            case PlayerStateEnum.NormalSlam:
            case PlayerStateEnum.AwakenSlam:
                _cooldownCoroutines[playerState] = StartCoroutine(SkillCooldownCoroutine(playerState, _slamSkillCooldownImage, GameManager.Instance.PlayerInstance.PlayerStatData.GetSlamCooldown()));

                break;

            case PlayerStateEnum.NormalChargeAttack:
            case PlayerStateEnum.AwakenChargeAttack:
                _cooldownCoroutines[playerState] = StartCoroutine(SkillCooldownCoroutine(playerState, _chargingSkillCooldownImage, GameManager.Instance.PlayerInstance.PlayerStatData.GetChargingAttackCooldown()));

                break;

            default:
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
        if (_player.BuffCompo.GetBuffState(BuffType.Rune_Acceleration_Gabriel))
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.maxAwakenGauge.AddModifier(-20);
        }
        else
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.maxAwakenGauge.RemoveModifier(20);
        }
        float currentAwakenGauge = GameManager.Instance.PlayerInstance.CurrentAwakenGauge;
        float maxAwakenGauge = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxAwakenGauge();

        _awakenGaugeSlider.value = currentAwakenGauge;
        _awakenGaugeText.text = $"{(int)(currentAwakenGauge / maxAwakenGauge * 100f)}%";
    }

    public void UpdateChargingGauge()
    {
        float currentChargingTime = GameManager.Instance.PlayerInstance.CurrentChargingTime;
        float maxChargingTime = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxChargingTime();
        _chargingTimeSlider.value = currentChargingTime;
        _chargingTimeText.text = $"{(int)(currentChargingTime / maxChargingTime * 100f)}%";
    }

    private IEnumerator BuffDurationCoroutine(BuffType buffType, Image buffDurationImage, float duration = 0)
    {
        if (duration > 0f)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                buffDurationImage.fillAmount = Mathf.Clamp01(elapsed / duration);

                yield return null;
            }
        }
        else
        {
            while (_player.BuffCompo.GetBuffState(buffType))
            {
                yield return null;
            }
        }

        buffDurationImage.fillAmount = 1f;

        buffDurationImage.transform.parent.parent.parent.gameObject.SetActive(false);
    }

    private IEnumerator SkillCooldownCoroutine(PlayerStateEnum playerState, Image skillCooldownImage, float cooldown)
    {
        float elapsed = 0f;

        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            skillCooldownImage.fillAmount = Mathf.Clamp01(1f - (elapsed / cooldown));

            yield return null;
        }

        skillCooldownImage.fillAmount = 0f;
        _cooldownCoroutines[playerState] = null;
    }
}