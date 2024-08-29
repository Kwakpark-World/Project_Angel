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
    private Image _dashSkillIconImage;
    [SerializeField]
    private Sprite _dashSkillNormalRollIcon;
    [SerializeField]
    private Sprite _dashSkillAwakenRollIcon;
    [SerializeField]
    private Sprite _dashSkillNormalDashIcon;
    [SerializeField]
    private Sprite _dashSkillAwakenDashIcon;
    [SerializeField]
    private Image _dashSkillCooldownImage;

    [SerializeField]
    private Image _chargeSkillCooldownImage;

    [SerializeField]
    private Image _slamSkillCooldownImage;

    [SerializeField]
    private Image _whirlwindSkillCooldownImage;

    [Header("Player Stat")]
    [SerializeField]
    private Image _healthBarImage;

    [SerializeField]
    private Image _awakenGaugeImage;

    [SerializeField]
    private Image _chargingTimeImage;

    public Player PlayerReference { get; set; }
    private Dictionary<BuffType, Coroutine> _durationCoroutines = new Dictionary<BuffType, Coroutine>();

    private void Start()
    {
        foreach (BuffType buffType in Enum.GetValues(typeof(BuffType)))
        {
            _durationCoroutines[buffType] = null;
        }
    }

    public void ChangeSkillIcon(bool isAwaken)
    {
        if (!isAwaken)
        {
            _dashSkillIconImage.sprite = !PlayerReference.BuffCompo.GetBuffState(BuffType.Rune_Dash_1) ? _dashSkillNormalRollIcon : _dashSkillNormalDashIcon;
        }
        else
        {
            _dashSkillIconImage.sprite = !PlayerReference.BuffCompo.GetBuffState(BuffType.Rune_Dash_1) ? _dashSkillAwakenRollIcon : _dashSkillAwakenDashIcon;
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
                _poisonBuffDurationImage.transform.parent.gameObject.SetActive(true);
                _durationCoroutines[buffType] = StartCoroutine(BuffDurationCoroutine(buffType, _poisonBuffDurationImage, duration));

                break;

            case BuffType.Potion_Freeze:
                _freezeBuffDurationImage.transform.parent.gameObject.SetActive(true);
                _durationCoroutines[buffType] = StartCoroutine(BuffDurationCoroutine(buffType, _freezeBuffDurationImage, duration));

                break;

            case BuffType.Potion_Paralysis:
                _paralysisBuffDurationImage.transform.parent.gameObject.SetActive(true);
                _durationCoroutines[buffType] = StartCoroutine(BuffDurationCoroutine(buffType, _paralysisBuffDurationImage, duration));

                break;

            default:
                break;
        }
    }

    public void UpdateSkillCooldown(float dashLeftCooldown, float chargeLeftCooldown, float slamLeftCooldown, float whirlwindLeftCooldown)
    {
        _dashSkillCooldownImage.fillAmount = dashLeftCooldown;
        _chargeSkillCooldownImage.fillAmount = chargeLeftCooldown;
        _slamSkillCooldownImage.fillAmount = slamLeftCooldown;
        _whirlwindSkillCooldownImage.fillAmount = whirlwindLeftCooldown;
    }

    public void UpdateHealth()
    {
        float currentHealth = GameManager.Instance.PlayerInstance.CurrentHealth;
        float maxHealth = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxHealth();
        _healthBarImage.fillAmount = currentHealth / maxHealth;
        //_healthText.text = $"{Mathf.Clamp(currentHealth, 0f, maxHealth)} / {maxHealth}";
    }

    public void UpdateAwakenGauge()
    {
        float currentAwakenGauge = GameManager.Instance.PlayerInstance.CurrentAwakenGauge;
        float maxAwakenGauge = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxAwakenGauge();
        _awakenGaugeImage.fillAmount = currentAwakenGauge / maxAwakenGauge;
        //_awakenGaugeText.text = $"{(int)(currentAwakenGauge / maxAwakenGauge * 100f)}%";
    }

    public void UpdateChargeGauge()
    {
        float currentChargeTime = GameManager.Instance.PlayerInstance.CurrentChargeTime;
        float maxChargeTime = GameManager.Instance.PlayerInstance.PlayerStatData.GetMaxChargeTime();
        _chargingTimeImage.fillAmount = currentChargeTime / maxChargeTime;
        //_chargingTimeText.text = $"{(int)(currentChargeTime / maxChargeTime * 100f)}%";
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

        buffDurationImage.transform.parent.gameObject.SetActive(false);
    }
}
