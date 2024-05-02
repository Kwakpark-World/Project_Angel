using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{
    public float XInput { get; private set; }
    public float YInput { get; private set; }

    public float MouseWheel { get; private set; } = 0f;

    public bool isDefense { get; private set; }
    public bool isCharge;
    public Vector2 MousePos;

    public event Action DashEvent;
    public event Action MeleeAttackEvent;
    public event Action SlamSkillEvent;
    public event Action AwakeningSkillEvent;


    private Controls _controls;
    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }

    public void OnXMovement(InputAction.CallbackContext context)
    {
        XInput = context.ReadValue<float>();
    }

    public void OnYMovement(InputAction.CallbackContext context)
    {
        YInput = context.ReadValue<float>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DashEvent?.Invoke();
        }
    }

    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        isCharge = context.performed;
        if (context.performed)
        {
            MeleeAttackEvent?.Invoke();
        }
    }

    public void OnDefense(InputAction.CallbackContext context)
    {
        isDefense = context.performed;
    }

    public void OnQSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SlamSkillEvent?.Invoke();
        }
    }

    public void OnESkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AwakeningSkillEvent?.Invoke();
        }
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        MousePos = context.ReadValue<Vector2>();
    }

    public void OnMouseWheel(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (value > 0f)
        {
            MouseWheel = -1f;
        }
        else if (value < 0f)
        {
            MouseWheel = 1f;
        }
        else
        {
            MouseWheel = 0f;
        }
    }
}
