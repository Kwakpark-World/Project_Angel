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

    private void Update()
    {
        UpdateHp();
        UpdateAwakenGage();
        UpdateChargeGage();
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
}
