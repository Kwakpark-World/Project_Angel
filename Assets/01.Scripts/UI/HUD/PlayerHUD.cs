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
    private Sprite _normalDefenseSkillIcon;
    [SerializeField]
    private Sprite _awakenDefenseSkillIcon;
    [SerializeField]
    private Image _defenseSkillIconImage;
    [SerializeField]
    private Image _defenseSkillCooldownImage;

    [SerializeField]
    private Sprite _normalChargingSkillIcon;
    [SerializeField]
    private Sprite _awakenChargingSkillIcon;
    [SerializeField]
    private Image _chargingSkillIconImage;
    [SerializeField]
    private Image _chargingSkillCooldownImage;

    [SerializeField]
    private Sprite _normalSlamSkillIcon;
    [SerializeField]
    private List<Sprite> _awakenSlamSkillIcons;
    [SerializeField]
    private Image _slamSkillIconImage;
    [SerializeField]
    private Image _slamSkillCooldownImage;

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

    public Player PlayerReference { get; set; }
    private Dictionary<BuffType, Coroutine> _durationCoroutines = new Dictionary<BuffType, Coroutine>();

    private void Start()
    {
        foreach (BuffType buffType in Enum.GetValues(typeof(BuffType)))
        {
            _durationCoroutines[buffType] = null;
        }
    }

    public void SetNormalSkillIcon()
    {
        _defenseSkillIconImage.sprite = _normalDefenseSkillIcon;
        _chargingSkillIconImage.sprite = _normalChargingSkillIcon;
        _slamSkillIconImage.sprite = _normalSlamSkillIcon;
    }

    public void SetAwakenSkillIcon()
    {
        _defenseSkillIconImage.sprite = _awakenDefenseSkillIcon;
        _chargingSkillIconImage.sprite = _awakenChargingSkillIcon;
        _slamSkillIconImage.sprite = _awakenSlamSkillIcons[0];
    }

    public void UpdateSkillComboIcon(int comboCounter)
    {
        if (_slamSkillIconImage.sprite == _awakenSlamSkillIcons[(comboCounter - 1) % 3])
        {
            _slamSkillIconImage.sprite = _awakenSlamSkillIcons[comboCounter % 3];
        }
    }

    public void StartBuffDuration(BuffType buffType, float duration = 0)
    {
        if (_durationCoroutines[buffType] != null)
        {
            StopCoroutine(_durationCoroutines[buffType]);
        }

        switch (buffType)
        {
            case BuffType.Potion_Poison:
                _poisonBuffDurationImage.transform.parent.parent.parent.gameObject.SetActive(true);
                _durationCoroutines[buffType] = StartCoroutine(BuffDurationCoroutine(buffType, _poisonBuffDurationImage, duration));

                break;

            case BuffType.Potion_Freeze:
                _freezeBuffDurationImage.transform.parent.parent.parent.gameObject.SetActive(true);
                _durationCoroutines[buffType] = StartCoroutine(BuffDurationCoroutine(buffType, _freezeBuffDurationImage, duration));

                break;

            case BuffType.Potion_Paralysis:
                _paralysisBuffDurationImage.transform.parent.parent.parent.gameObject.SetActive(true);
                _durationCoroutines[buffType] = StartCoroutine(BuffDurationCoroutine(buffType, _paralysisBuffDurationImage, duration));

                break;

            default:
                break;
        }
    }

    public void UpdateSkillCooldown(float dashLeftCooldown, float defenseLeftCooldown, float slamLeftCooldown, float chargingLeftCooldown)
    {
        _defenseSkillCooldownImage.fillAmount = defenseLeftCooldown;
        _chargingSkillCooldownImage.fillAmount = chargingLeftCooldown;
        _slamSkillCooldownImage.fillAmount = slamLeftCooldown;
    }

    public void UpdateHealth()
    {
        float currentHealth = GameManager.Instance.PlayerInstance.CurrentHealth;
        float maxHealth = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxHealth();
        _healthBarImage.fillAmount = currentHealth / maxHealth;
        _healthText.text = $"{Mathf.Clamp(currentHealth, 0f, maxHealth)} / {maxHealth}";
    }

    public void UpdateAwakenGauge()
    {
        float currentAwakenGauge = GameManager.Instance.PlayerInstance.CurrentAwakenGauge;
        float maxAwakenGauge = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxAwakenGauge();
        _awakenGaugeSlider.value = currentAwakenGauge;
        _awakenGaugeSlider.maxValue = maxAwakenGauge;
        _awakenGaugeText.text = $"{(int)(currentAwakenGauge / maxAwakenGauge * 100f)}%";
    }

    public void UpdateChargingGauge()
    {
        float currentChargingTime = GameManager.Instance.PlayerInstance.CurrentChargingTime;
        float maxChargingTime = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxChargingTime();
        _chargingTimeSlider.value = currentChargingTime;
        _chargingTimeSlider.maxValue = maxChargingTime;
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
            while (PlayerReference.BuffCompo.GetBuffState(buffType))
            {
                yield return null;
            }
        }

        buffDurationImage.fillAmount = 1f;
        _durationCoroutines[buffType] = null;

        buffDurationImage.transform.parent.parent.parent.gameObject.SetActive(false);
    }
}
