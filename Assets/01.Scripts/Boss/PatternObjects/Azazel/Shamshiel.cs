using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shamshiel : Grigori
{
    protected override void Attack()
    {

    }

    protected override void Debuff()
    {
        // Give slowness effect to player.
        // Debug
        player.moveSpeed *= 0.75f;
    }
}
