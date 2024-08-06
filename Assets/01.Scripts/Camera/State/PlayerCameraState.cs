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

            /* 
            float currentXRotation = _player.transform.eulerAngles.x;

            float newXRotation = currentXRotation + -rotationAmountY;
            if (newXRotation > 180) newXRotation -= 360; 
            newXRotation = Mathf.Clamp(newXRotation, -45f, 45f); 
            _player.transform.eulerAngles = new Vector3(newXRotation, _player.transform.eulerAngles.y, _player.transform.eulerAngles.z);*/
        }
    }



    public void CameraAttack()
    {
        
    }


    /*private void SetRotation()
    {
        // 카메라의 Y축 회전 가져오기
        float cameraYRotation = transform.eulerAngles.y;

        // 플레이어 회전 벡터 가져오기
        Vector3 playerRotation = _player.transform.eulerAngles;

        // 플레이어의 Y축 회전을 카메라의 Y축 회전으로 설정
        playerRotation.y = cameraYRotation;

        // 플레이어의 회전 적용
        _player.transform.eulerAngles = playerRotation;

        // 입력 값 가져오기
        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        // 카메라의 앞 방향 및 오른쪽 방향 가져오기
        Vector3 cameraForward = transform.forward;
        Vector3 cameraRight = transform.right;

        // 원하는 방향 계산
        Vector3 desiredDirection = (cameraForward * yInput + cameraRight * xInput).normalized;

        // 원하는 방향이 0이 아닐 경우 회전 처리
        if (desiredDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRotation, Time.deltaTime * _player.PlayerStatData.GetRotateSpeed());
        }
    }*/


}
