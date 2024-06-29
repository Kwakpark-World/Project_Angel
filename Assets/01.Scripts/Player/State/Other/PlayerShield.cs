using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : PlayerAttackState
{
    public float shieldAmount = 10f;

    public PlayerShield(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }
    /*public void shieldOn()
    {
        if(_player.OnHit())
        {

        }
    }*/
}
