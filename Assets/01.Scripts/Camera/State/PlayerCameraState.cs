using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraState : CameraState
{
    private Player _player;

    public override CameraState RegisterCamera()
    {
        base.RegisterCamera();

        if (_player == null)
            _player = GameManager.Instance.PlayerInstance;

        return this;
    }

    private void Update()
    {
        Vector3 dir = new Vector3(Mouse.current.delta.x.ReadValue(), 0, 0);
        dir *= _player.PlayerStatData.GetRotateSpeed();

        _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, _player.transform.rotation * Quaternion.LookRotation(dir), Time.deltaTime);
    }
}
