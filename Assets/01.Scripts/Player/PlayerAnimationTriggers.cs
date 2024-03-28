using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = transform.parent.GetComponent<Player>();
    }

    private void AnimationActionTrigger()
    {
        _player.AnimationActionTrigger();
    }

    private void AnimationEndTrigger()
    {
        _player.AnimationEndTrigger();
    }

    private void AnimationHitAbleTrigger()
    {
        _player.AnimationHitAbleTrigger();
    }
}
