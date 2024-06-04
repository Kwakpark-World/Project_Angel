using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerWalkState : PlayerGroundState
{
    private float _movementMultiplier = 30.0f;

    private float _maxStepHeight = 1f;
    private float _minStepDepth = 0.3f;
    private float _stairHeightPaddingMultiplier = 1.5f;
    private bool _isFirstStep = true;
    private float _firstStepVelocityDistanceMultiplier = 0.1f;
    private bool _isPlayerAscendingStairs = false;
    private bool _isPlayerDescendingStairs = false;
    private float _ascendingStairsMovementMultiplier = 0.35f;
    private float _descendingStairsMovementMultiplier = 0.7f;
    private float _maximumAngleOfApproachToAscend = 45.0f;
    private float _playerHalfHeightToGround = 0.0f;
    private float _maxAscendRayDistance = 0.0f;
    private float _maxDescnedRayDistance = 0.0f;
    private int _numberOfStepDetectRays = 0;
    private float _rayIncrementAmount = 0.0f;

    private float _gravityGrounded = -1.0f;
    private float _maxSlopeAngle = 47.5f;

    public PlayerWalkState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _maxAscendRayDistance = _maxStepHeight / Mathf.Cos(_maximumAngleOfApproachToAscend * Mathf.Deg2Rad);
        _maxDescnedRayDistance = _maxStepHeight / Mathf.Cos(80.0f  * Mathf.Deg2Rad);

        _numberOfStepDetectRays = Mathf.RoundToInt(((_maxStepHeight * 100.0f) * 0.5f) + 1.0f);
        _rayIncrementAmount = _maxStepHeight / _numberOfStepDetectRays;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        Vector3 moveDir = SetDirection(xInput, yInput);
        moveDir = PlayerStair(moveDir);
        moveDir = PlayerSlope(moveDir);

        _player.SetVelocity(moveDir);

        if (Mathf.Abs(xInput) < 0.05f && Mathf.Abs(yInput) < 0.05f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private Vector3 SetDirection(float xInput, float yInput)
    {
        Vector3 moveDir = new Vector3(xInput, 0, yInput).normalized;

        moveDir = (Quaternion.Euler(0, CameraManager.Instance.GetCameraByType(CameraType.PlayerCam).transform.eulerAngles.y, 0) * moveDir).normalized;
        moveDir *= _player.PlayerStatData.GetMoveSpeed();

        if (moveDir.sqrMagnitude > 0)
        {
            _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * _player.PlayerStatData.GetRotateSpeed());
        }

        moveDir.y = _rigidbody.velocity.y;
        return moveDir;
    }

    private Vector3 PlayerStair(Vector3 moveDir)
    {
        Vector3 calculatedStepInput = moveDir;

        _playerHalfHeightToGround = _player.ColliderCompo.bounds.extents.y;
        if (_player.playerCenterToGroundDistance < _player.ColliderCompo.bounds.extents.y)
        {
            _playerHalfHeightToGround = _player.playerCenterToGroundDistance;
        }
        calculatedStepInput = AscendStairs(calculatedStepInput, moveDir);
        if (!_isPlayerAscendingStairs)
            calculatedStepInput = DescendStairs(calculatedStepInput, moveDir);

        return calculatedStepInput; 
    }

    private Vector3 AscendStairs(Vector3 calculatedStepInput, Vector3 moveDir)
    {
        if (_player.IsMovePressed())
        {
            float calculatedVelDistance = _isFirstStep ? (_rigidbody.velocity.magnitude * _firstStepVelocityDistanceMultiplier) + _player.ColliderCompo.radius : _player.ColliderCompo.radius;

            float ray = 0.0f;
            List<RaycastHit> raysThatHit = new List<RaycastHit>();
            for (int x = 1; x <= _numberOfStepDetectRays; x++, ray += _rayIncrementAmount)
            {
                Vector3 rayLower = new Vector3(_rigidbody.position.x, ((_rigidbody.position.y - _playerHalfHeightToGround) + ray), _rigidbody.position.z);
                RaycastHit hitLower;
                if (Physics.Raycast(rayLower, _rigidbody.transform.TransformDirection(moveDir), out hitLower, calculatedVelDistance + _maxAscendRayDistance))
                {
                    float stairSlopeAngle = Vector3.Angle(hitLower.normal, _rigidbody.transform.up);
                    if (stairSlopeAngle == 90.0f)
                    {
                        raysThatHit.Add(hitLower);
                    }
                }
            }

            if (raysThatHit.Count > 0)
            {
                Vector3 rayUpper = new Vector3(_rigidbody.position.x, (((_rigidbody.position.y - _playerHalfHeightToGround) + _maxStepHeight) + _rayIncrementAmount), _rigidbody.position.z);
                RaycastHit hitUpper;
                Physics.Raycast(rayUpper, _rigidbody.transform.TransformDirection(moveDir), out hitUpper, calculatedVelDistance + (_maxAscendRayDistance * 2.0f));
                if (!hitUpper.collider || (hitUpper.distance - raysThatHit[0].distance) > _minStepDepth)
                {
                    if (Vector3.Angle(raysThatHit[0].normal, _rigidbody.transform.TransformDirection(moveDir)) <= _maximumAngleOfApproachToAscend){
                        Debug.DrawRay(rayUpper, _rigidbody.transform.TransformDirection(moveDir), Color.red, 5.0f);

                        _isPlayerAscendingStairs = true;
                        Vector3 playerRelX = Vector3.Cross(moveDir, Vector3.up);

                        if (_isFirstStep)
                        {
                            calculatedStepInput = Quaternion.AngleAxis(45.0f, playerRelX) * calculatedStepInput;
                            _isFirstStep = false;
                        }
                        else
                        {
                            float stairHeight = raysThatHit.Count * _rayIncrementAmount * _stairHeightPaddingMultiplier;

                            float avgDistance = 0.0f;
                            foreach (RaycastHit r in raysThatHit)
                            {
                                avgDistance += r.distance;
                            }
                            avgDistance /= raysThatHit.Count;

                            float tanAngle = Mathf.Atan2(stairHeight, avgDistance) * Mathf.Rad2Deg;
                            calculatedStepInput = Quaternion.AngleAxis(tanAngle, playerRelX) * calculatedStepInput;
                            calculatedStepInput *= _ascendingStairsMovementMultiplier;
                        }
                    }
                    else
                    {
                        _isPlayerAscendingStairs = false;
                        _isFirstStep = true;
                    }
                }
                else
                {
                    _isPlayerAscendingStairs = false;
                    _isFirstStep = true;
                }
            }
            else
            {
                _isPlayerAscendingStairs = false;
                _isFirstStep = true;
            }
        }
        else
        {
            _isPlayerAscendingStairs = false;
            _isFirstStep = true;
        }
        return calculatedStepInput;
    }

    private Vector3 DescendStairs(Vector3 calculatedStepInput, Vector3 moveDir)
    {
        if (_player.IsMovePressed())
        {
            float ray = 0.0f;
            List<RaycastHit> raysThatHit = new List<RaycastHit>();
            for (int x = 1; x <= _numberOfStepDetectRays; x++, ray += _rayIncrementAmount)
            {
                Vector3 rayLower = new Vector3(_rigidbody.position.x, ((_rigidbody.position.y - _playerHalfHeightToGround) + ray), _rigidbody.position.z);
                RaycastHit hitLower;
                if (Physics.Raycast(rayLower, _rigidbody.transform.TransformDirection(-moveDir), out hitLower, _player.ColliderCompo.radius + _maxDescnedRayDistance))
                {
                    float stairSlopeAngle = Vector3.Angle(hitLower.normal, _rigidbody.transform.up);
                    if (stairSlopeAngle == 90.0f)
                    {
                        raysThatHit.Add(hitLower);
                    }
                }
            }

            if (raysThatHit.Count > 0)
            {
                Vector3 rayUpper = new Vector3(_rigidbody.position.x, (((_rigidbody.position.y - _playerHalfHeightToGround) + _maxStepHeight) + _rayIncrementAmount), _rigidbody.position.z);
                RaycastHit hitUpper;
                Physics.Raycast(rayUpper, _rigidbody.transform.TransformDirection(-moveDir), out hitUpper, _player.ColliderCompo.radius + (_maxDescnedRayDistance * 2.0f));
                if (!hitUpper.collider || (hitUpper.distance - raysThatHit[0].distance) > _minStepDepth)
                {
                    if (!_player.IsGroundDetected() && hitUpper.distance < _player.ColliderCompo.radius + (_maxDescnedRayDistance * 2.0f))
                    {
                        Debug.DrawRay(rayUpper, _rigidbody.transform.TransformDirection(-moveDir), Color.red, 5.0f);

                        _isPlayerDescendingStairs = true;
                        Vector3 playerRelX = Vector3.Cross(moveDir, Vector3.up);
                        
                        float stairHeight = raysThatHit.Count * _rayIncrementAmount * _stairHeightPaddingMultiplier;

                        float avgDistance = 0.0f;
                        foreach (RaycastHit r in raysThatHit)
                        {
                            avgDistance += r.distance;
                        }
                        avgDistance /= raysThatHit.Count;

                        float tanAngle = Mathf.Atan2(stairHeight, avgDistance) * Mathf.Rad2Deg;
                        calculatedStepInput = Quaternion.AngleAxis(tanAngle - 90.0f, playerRelX) * calculatedStepInput;
                        calculatedStepInput *= _descendingStairsMovementMultiplier;
                        
                    }
                    else
                    {
                        _isPlayerDescendingStairs = false;
                    }
                }
                else
                {
                    _isPlayerDescendingStairs = false;
                }
            }
            else
            {
                _isPlayerDescendingStairs = false;
            }
        }
        else
        {
            _isPlayerDescendingStairs = false;
        }
        return calculatedStepInput;
    }

    private Vector3 PlayerSlope(Vector3 moveDir)
    {
        Vector3 calculatedPlayerMovement = moveDir;

        if (_player.IsGroundDetected() && !_isPlayerAscendingStairs && !_isPlayerDescendingStairs)
        {
            Vector3 localGroundCheckHitNormal = _rigidbody.transform.InverseTransformDirection(_player.groundCheckHit.normal);

            float groundSlopeAngle = Vector3.Angle(localGroundCheckHitNormal, _rigidbody.transform.up);
            if (groundSlopeAngle == 0.0f)
            {
                if (_player.IsMovePressed())
                {
                    RaycastHit rayHit;
                    float rayCalculatedRayHeight = _rigidbody.position.y - _player.playerCenterToGroundDistance + _player.groundCheckDistanceTolerance;
                    Vector3 rayOrigin = new Vector3(_rigidbody.position.x, rayCalculatedRayHeight, _rigidbody.position.z);
                    if (Physics.Raycast(rayOrigin, _rigidbody.transform.TransformDirection(calculatedPlayerMovement), out rayHit, 0.75f))
                    {
                        if (Vector3.Angle(rayHit.normal, _rigidbody.transform.up) > _maxSlopeAngle)
                        {
                            calculatedPlayerMovement.y = -_movementMultiplier;
                        }
                    }
                }

                if (calculatedPlayerMovement.y == 0.0f)
                {
                    calculatedPlayerMovement.y = _gravityGrounded;
                }
            }
            else
            {
                Quaternion slopeAngleRotation = Quaternion.FromToRotation(_rigidbody.transform.up, localGroundCheckHitNormal);
                calculatedPlayerMovement = slopeAngleRotation * calculatedPlayerMovement;

                float relativeSlopeAngle = Vector3.Angle(calculatedPlayerMovement, _rigidbody.transform.up) - 90.0f;
                calculatedPlayerMovement += calculatedPlayerMovement * (relativeSlopeAngle / 90.0f);

                if (groundSlopeAngle < _maxSlopeAngle)
                {
                    if (_player.IsMovePressed())
                    {
                        calculatedPlayerMovement.y += _gravityGrounded;
                    }
                }
                else
                {
                    float calculatedSlopeGravity = groundSlopeAngle * -0.2f;
                    if (calculatedSlopeGravity < calculatedPlayerMovement.y)
                    {
                        calculatedPlayerMovement.y = calculatedSlopeGravity;
                    }
                }
            }
        }

        return calculatedPlayerMovement;
    }
}
