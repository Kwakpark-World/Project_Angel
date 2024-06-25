using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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
        if (_player.IsDie) return;

        SetRotation();
    }

    private void SetRotation()
    {
        //float cameraYRotation = transform.eulerAngles.y;
        //Vector3 playerRotation = _player.transform.eulerAngles;
        //
        //playerRotation.y = cameraYRotation;
        //
        //_player.transform.eulerAngles = playerRotation;

        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        Vector3 cameraForward = transform.forward;
        Vector3 cameraRight = transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 desiredDirection = (cameraForward * yInput + cameraRight * xInput).normalized;

        if (desiredDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRotation, Time.deltaTime * _player.PlayerStatData.GetRotateSpeed());
        }
    }

}
