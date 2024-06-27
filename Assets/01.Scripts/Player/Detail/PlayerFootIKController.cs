using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootIKController : MonoBehaviour
{
    private Player _player;
    public float DistanceToGround;

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
        if (_player.AnimatorCompo)
        {
            _player.AnimatorCompo.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            _player.AnimatorCompo.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            _player.AnimatorCompo.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            _player.AnimatorCompo.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            RaycastHit hit;
            Ray ray = new Ray(_player.AnimatorCompo.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, _player.whatIsGround))
            {
                if (CompareGroundLayer(hit.transform.gameObject.layer))
                {
                    Vector3 footPos = hit.point;
                    footPos.y += DistanceToGround;
                    _player.AnimatorCompo.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
                    //_player.AnimatorCompo.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(_player.transform.forward, hit.normal));
                }
            }

            ray = new Ray(_player.AnimatorCompo.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, _player.whatIsGround))
            {
                if (CompareGroundLayer(hit.transform.gameObject.layer))
                {
                    Vector3 footPos = hit.point;
                    footPos.y += DistanceToGround;
                    _player.AnimatorCompo.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
                    //_player.AnimatorCompo.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(_player.transform.forward, hit.normal));
                }

            }
        }
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