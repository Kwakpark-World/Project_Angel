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

        switch (buffType)
        {
            case BuffType.Rune_Dash_1:
                playerInstance.isRollToDash = true;
                break;

            case BuffType.Rune_Dash_2:
                playerInstance.isRollAttack = true;
                break;

            case BuffType.Rune_Dash_3:
                playerInstance.isRollKnockback = true;
                break;

            case BuffType.Rune_Dash_4:
                playerInstance.isRollOnceMore = true;
                break;

            default:
                break;
        }
    }

    public void Rune_Charge()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        switch (buffType)
        {
            case BuffType.Rune_Charge_1:
                playerInstance.isChargingTripleSting = true;
                break;

            case BuffType.Rune_Charge_2:
                playerInstance.isChargingMultipleSting = true;
                break;

            case BuffType.Rune_Charge_3:
                playerInstance.isChargingSlashOnceMore = true;
                break;

            case BuffType.Rune_Charge_4:
                playerInstance.isChargingSwordAura = true;
                break;

            default:
                break;
        }
    }

    /*public void Rune_Slam()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        switch (buffType)
        {
            case BuffType.Rune_Slam_1:
                playerInstance.isSlamEarthquake = true;
                break;

            case BuffType.Rune_Slam_2:
                playerInstance. = true;
                break;

            case BuffType.Rune_Slam_3:
                playerInstance. = true;
                break;

            case BuffType.Rune_Slam_4:
                playerInstance.isSlamSixTimeSlam = true;
                break;

            default:
                break;
        }
    }*/

    public void Rune_Whirlwind()
    {
        var playerInstance = GameManager.Instance.PlayerInstance;

        switch (buffType)
        {
            case BuffType.Rune_Whirlwind_1:
                playerInstance.isWhirlwindShockWave = true;
                break;

            case BuffType.Rune_Whirlwind_2:
                playerInstance.isWhirlwindMoveAble = true;
                break;

            case BuffType.Rune_Whirlwind_3:
                playerInstance.isWhirlwindPullEnemies = true;
                break;

            case BuffType.Rune_Whirlwind_4:
                playerInstance.isWhirlwindRangeUp = true;
                break;

            default:
                break;
        }
    }

}
