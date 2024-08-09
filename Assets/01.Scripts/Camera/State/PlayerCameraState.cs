using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        if (_player.IsDie) return;

        SetAttackRotation();
        //SetRotation();
    }

    //
    private void SetAttackRotation()
    {
        if (!GameManager.Instance.PlayerInstance.IsAttack)
        {
            if (IsCamRotateStop) return;

            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            if (mouseDelta.x == 0 && mouseDelta.y == 0) return; // X와 Y 모두 0일 경우 반환

            // X축 회전
            float rotationAmountX = mouseDelta.x * Time.deltaTime * _player.PlayerStatData.GetXRotateSpeed();
            if (CameraManager.Instance.IsXReverse)
                rotationAmountX *= -1f;

            _player.transform.Rotate(Vector3.up, rotationAmountX);

            float rotationAmountY = mouseDelta.y * Time.deltaTime * _player.PlayerStatData.GetYRotateSpeed() * -1f;
            if (CameraManager.Instance.IsYReverse)
                rotationAmountY *= -1f;

            if (_camera.TryGetComponent<CinemachineVirtualCamera>(out CinemachineVirtualCamera cam))
            {
                var transposer = cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

                if (transposer.ShoulderOffset.y + rotationAmountY < 0)
                {
                    transposer.ShoulderOffset.y = 0;
                }
                else if (transposer.ShoulderOffset.y + rotationAmountY > 6.5f)
                {
                    transposer.ShoulderOffset.y = 6.5f;
                }
                else
                    transposer.ShoulderOffset.y += rotationAmountY;
            }
        }
        
    }
}
