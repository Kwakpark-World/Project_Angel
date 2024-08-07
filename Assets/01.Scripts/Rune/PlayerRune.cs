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

        if (buffType == BuffType.Rune_Dash_1)
        {
            playerInstance.isRollToDash = true;
        }
        if (buffType == BuffType.Rune_Dash_2)
        {
            playerInstance.isRollAttack = true;
        }
        if (buffType == BuffType.Rune_Dash_3)
        {
            playerInstance.isRollKnockback = true;
        }
        if (buffType == BuffType.Rune_Dash_4)
        {
            playerInstance.isRollOnceMore = true;
        }
    }

    public void Rune_Charge()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        if (buffType == BuffType.Rune_Charge_1)
        {
            playerInstance.isChargingTripleSting = true;
        }
        if (buffType == BuffType.Rune_Charge_2)
        {
            playerInstance.isChargingMultipleSting = true;
        }
        if (buffType == BuffType.Rune_Charge_3)
        {
            playerInstance.isChargingSlashOnceMore = true;
        }
        if (buffType == BuffType.Rune_Charge_4)
        {
            playerInstance.isChargingSwordAura = true;
        }
    }

    public void Rune_Slam()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        if (buffType == BuffType.Rune_Slam_1)
        {
            playerInstance.isSlamEarthquake = true;
        }
        if (buffType == BuffType.Rune_Slam_2)
        {
            playerInstance.isSlamStatic = true;
        }
        if (buffType == BuffType.Rune_Slam_3)
        {
            playerInstance.isSlamFloorEnd = true;
        }
        if (buffType == BuffType.Rune_Slam_4)
        {
            playerInstance.isSlamSixTimeSlam = true;
        }
    }

    public void Rune_Whirlwind()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        if (buffType == BuffType.Rune_Whirlwind_1)
        {
            playerInstance.isWhirlwindShockWave = true;
        }
        if (buffType == BuffType.Rune_Whirlwind_2)
        {
            playerInstance.isWhirlwindMoveAble = true;
        }
        if (buffType == BuffType.Rune_Whirlwind_3)
        {
            playerInstance.isWhirlwindPullEnemies = true;
        }
        if (buffType == BuffType.Rune_Whirlwind_4)
        {
            playerInstance.isWhirlwindRangeUp = true;
        }
    }

}
