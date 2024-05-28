using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : PlayerCheckTrap
{
    [Header("Check Parameter")]
    public Vector3 checkCenter;
    public Vector3 checkSize;
    public Vector3 checkRotation;

    [Header("Attack Parameter")]
    public Vector3 attackCenter;
    public Vector3 attackSize;
    public Vector3 attackRotation;

    public float onSpikeTimeDelay = 5f;
    public float upScale;

    private float _damage = 8f;
    private Vector3 _defaultPosition;
    private Coroutine _spikeCoroutine = null;

    private bool _isOnSpike = false;

    protected override void Awake()
    {
        base.Awake();

        // debug
        SetPlayerRangeParameter();

        _isOnTrap = true;
        _defaultPosition = transform.position;

        // debug    
    }

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        _isOnSpike = false;
        _defaultPosition = transform.position;
    }

    protected override void StartTrap()
    {
        // 가시 발판 빤짝하기 (나오기 전 나올거라 알림)
        OnSpike();

        StartDelayAction(()=> base.StartTrap());
    }

    protected override void PlayTrap()
    {
        AttackObject();

        StartDelayAction(onSpikeTimeDelay, ()=> base.PlayTrap());
    }

    protected override void EndTrap()
    {
        OffSpike();

        base.EndTrap();
    }

    private void OnSpike()
    {
        if (_spikeCoroutine != null) return;
        if (_isOnSpike) return;

        Vector3 targetPos = transform.position + transform.up * upScale;
        float duration = 0.5f;

        _isOnSpike = true;
        _spikeCoroutine = StartCoroutine(MoveSpike(targetPos, duration));
    }
    
    private void OffSpike()
    {
        float duration = 50f;

        _prevRunTime = Time.time;

        _isOnSpike = false;
        StartCoroutine(MoveSpike(_defaultPosition, duration));

        _spikeCoroutine = null;
    }

    protected override void SetPlayerRangeParameter()
    {
        SetCheckParameter();
        SetAttackParameter();

        _trapDamage = _damage;
    }

    private void SetCheckParameter()
    {
        _playerCheckCenter = transform.position + checkCenter;
        _playerCheckHalfSize = checkSize / 2;
        _playerCheckRotation = transform.rotation * Quaternion.Euler(checkRotation);
    }
    
    private void SetAttackParameter()
    {
        _attackCenter = transform.position + attackCenter;
        _attackHalfSize = attackSize / 2;
        _attackRotation = transform.rotation * Quaternion.Euler(attackRotation);
    }

    private void OnDrawGizmos()
    {

        // PlayerCheck Range
        Gizmos.color = Color.green;

        Gizmos.matrix = Matrix4x4.Rotate(transform.rotation * Quaternion.Euler(checkRotation));

        Gizmos.DrawWireCube(transform.position + checkCenter, checkSize / 2);

        // PlayerAttack Range
        Gizmos.color = Color.red;

        Gizmos.matrix = Matrix4x4.Rotate(transform.rotation * Quaternion.Euler(attackRotation));

        Gizmos.DrawWireCube(transform.position + attackCenter, attackSize / 2);
    }

    private IEnumerator MoveSpike(Vector3 targetPos, float duration)
    {
        float delta = 0;

        while (Mathf.Abs(targetPos.y - transform.position.y) > 0f)
        {
            float t = delta / duration;

            transform.position = Vector3.LerpUnclamped(transform.position, targetPos, t);
            delta += Time.deltaTime;
            yield return null;
        }

        DelayActionStop();
    }
}
