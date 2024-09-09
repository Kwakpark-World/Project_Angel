using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRune : MonoBehaviour
{
    private BuffType buffType;
    private void Update()
    {
        
    }

    public void Rune_Dash()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        if (buffType == BuffType.Rune_Dash_Hermes)
        {
            playerInstance.isRollToDash = true;
        }
        if (buffType == BuffType.Rune_Dash_Hermóðr)
        {
            playerInstance.isRollAttack = true;
        }
        if (buffType == BuffType.Rune_Dash_Horus)
        {
            playerInstance.isRollKnockback = true;
        }
        if (buffType == BuffType.Rune_Dash_Gabriel)
        {
            playerInstance.isRollOnceMore = true;
        }
    }

    public void Rune_Charge()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        if (buffType == BuffType.Rune_Charge_Ares)
        {
            playerInstance.isChargingTripleSting = true;
        }
        if (buffType == BuffType.Rune_Charge_Týr)
        {
            playerInstance.isChargingMultipleSting = true;
        }
        if (buffType == BuffType.Rune_Charge_Neith)
        {
            playerInstance.isChargingSlashOnceMore = true;
        }
        if (buffType == BuffType.Rune_Charge_Michael)
        {
            playerInstance.isChargingSwordAura = true;
        }
    }

    public void Rune_Slam()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        if (buffType == BuffType.Rune_Slam_Cronus)
        {
            playerInstance.isSlamEarthquake = true;
        }
        if (buffType == BuffType.Rune_Slam_Thor)
        {
            playerInstance.isSlamStatic = true;
        }
        if (buffType == BuffType.Rune_Slam_Geb)
        {
            playerInstance.isSlamFloorEnd = true;
        }
        if (buffType == BuffType.Rune_Slam_Uriel)
        {
            playerInstance.isSlamSixTimeSlam = true;
        }
    }

    public void Rune_Whirlwind()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        if (buffType == BuffType.Rune_Whirlwind_Hades)
        {
            playerInstance.isWhirlwindShockWave = true;
        }
        if (buffType == BuffType.Rune_Whirlwind_Víðarr)
        {
            playerInstance.isWhirlwindMoveAble = true;
        }
        if (buffType == BuffType.Rune_Whirlwind_Anubis)
        {
            playerInstance.isWhirlwindPullEnemies = true;
        }
        if (buffType == BuffType.Rune_Whirlwind_Sariel)
        {
            playerInstance.isWhirlwindRangeUp = true;
        }
    }

}
