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
            if (mouseDelta.x == 0 && mouseDelta.y == 0) return; // X�� Y ��� 0�� ��� ��ȯ

            // X�� ȸ��
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
        // ī�޶��� Y�� ȸ�� ��������
        float cameraYRotation = transform.eulerAngles.y;

        // �÷��̾� ȸ�� ���� ��������
        Vector3 playerRotation = _player.transform.eulerAngles;

        // �÷��̾��� Y�� ȸ���� ī�޶��� Y�� ȸ������ ����
        playerRotation.y = cameraYRotation;

        // �÷��̾��� ȸ�� ����
        _player.transform.eulerAngles = playerRotation;

        // �Է� �� ��������
        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        // ī�޶��� �� ���� �� ������ ���� ��������
        Vector3 cameraForward = transform.forward;
        Vector3 cameraRight = transform.right;

        // ���ϴ� ���� ���
        Vector3 desiredDirection = (cameraForward * yInput + cameraRight * xInput).normalized;

        // ���ϴ� ������ 0�� �ƴ� ��� ȸ�� ó��
        if (desiredDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRotation, Time.deltaTime * _player.PlayerStatData.GetRotateSpeed());
        }
    }*/


}
