using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootIKController : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        if (transform.parent.TryGetComponent<Player>(out Player player))
            _player = player;
        else
        {
            _player = GameManager.Instance.PlayerInstance;

            if (_player == null)
                Debug.Log("Player is null");
        }     
    }

    private void OnAnimatorIK()
    {

    }

    private bool CompareGroundLayer(int value)
    {
        int compareValue = (int)_player.whatIsGround;
        int valueToLayerValue = 1 << value;

        int resultValue = compareValue & valueToLayerValue;

        bool result = false;
        if (valueToLayerValue == resultValue) 
        {
            result = true;
        }

        return result;
    }
}
