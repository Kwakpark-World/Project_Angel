using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    public GameObject spearObj;

    public float onSpikeTimeDelay = 5f;
    public float upScale;

    private float _damage = 8f;
    private Vector3 _defaultPosition;

    private bool _isOnSpike = false;
    private Coroutine _spikeCoroutine = null;

    /* Debug
    protected override void Awake()
    {
        base.Awake();

        // debug
        SetPlayerRangeParameter();

        _isOnTrap = true;
        _defaultPosition = transform.position;

        // debug    
    }
    */

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        _isOnSpike = false;
        _defaultPosition = spearObj.transform.position;
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

        base.PlayTrap();
    }

    protected override void EndTrap()
    {
        OffSpike();

        StartDelayAction(()=> base.EndTrap());
    }

    private void OnSpike()
    {
        if (_spikeCoroutine != null ) return;
        if (_isOnSpike) return;

        Vector3 targetPos = spearObj.transform.position + (Vector3.up * upScale);
        float duration = 0.5f;

        Debug.Log(targetPos);

        _spikeCoroutine = StartCoroutine(OnSpikeMove(targetPos, duration, onSpikeTimeDelay));
    }
    
    private void OffSpike()
    {
        if (_spikeCoroutine != null )
        {
            StopCoroutine(_spikeCoroutine);
            _spikeCoroutine = null;
        }

        float duration = 0.5f;
        _spikeCoroutine = StartCoroutine(OffSpikeMove(duration));
    }

    private IEnumerator OnSpikeMove(Vector3 targetPos, float duration, float delay = 0)
    {
        float delta = 0;
        float t = 0;

        yield return new WaitForSeconds(delay);

        _isOnSpike = true;
        while (t <= 1)
        {
            t = delta / duration;
            spearObj.transform.position = Vector3.LerpUnclamped(spearObj.transform.position, targetPos, t);
            delta += Time.deltaTime;

            yield return null;
        }

        DelayActionStop();
        _spikeCoroutine = null;
    }

    private IEnumerator OffSpikeMove(float duration)
    {
        float delta = 0;
        float t = 0;
        while (t <= 1)
        {
            t = delta / duration;
            spearObj.transform.position = Vector3.LerpUnclamped(spearObj.transform.position, _defaultPosition, t);
            delta += Time.deltaTime;

            yield return null;
        }

        _isOnSpike = false;
        _prevRunTime = Time.time;
        _spikeCoroutine = null;
        DelayActionStop();

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
        _trapCoolTime = 2f;

        _attackCenter = transform.position + attackCenter;
        _attackHalfSize = attackSize;
        _attackRotation = transform.rotation * Quaternion.Euler(attackRotation);
    }

    private void OnDrawGizmos()
    {

        // PlayerCheck Range
        Gizmos.color = Color.green;

        Gizmos.matrix = Matrix4x4.Rotate(spearObj.transform.rotation * Quaternion.Euler(checkRotation));

        Vector3 pos = spearObj.transform.position + checkCenter;
        
        Gizmos.DrawWireCube(pos, checkSize);

        // PlayerAttack Range
        Gizmos.color = Color.red;

        Gizmos.matrix = Matrix4x4.Rotate(spearObj.transform.rotation * Quaternion.Euler(attackRotation));

        pos = spearObj.transform.position + attackCenter;
        
        Gizmos.DrawWireCube(pos, attackSize);
    }
}
