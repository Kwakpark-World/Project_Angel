using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour  
{
    [Header("Ground Checker")]
    [SerializeField] protected Transform _groundChecker;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected LayerMask _whatIsGround;
    [SerializeField] protected LayerMask _whatIsStair;

    [Header("Stair Checker")]
    [SerializeField] private Transform _stairUpperChecker;
    [SerializeField] private Transform _stairLowerChecker;
    [SerializeField] private float _stairUpperCheckDistance;
    [SerializeField] private float _stairLowerCheckDistance;

    [Header("Stair Parameters")]
    [SerializeField] private float _stairHeight;
    [SerializeField] private float _stairMoveSmooth;


    #region components
    [Space(30f), Header("Components")]
    public Animator AnimatorCompo;

    public Rigidbody RigidbodyCompo { get; private set; }
    public CapsuleCollider ColliderCompo { get; private set; }

    public Buff BuffCompo { get; private set; }
    [field: SerializeField] public PlayerStat PlayerStatData { get; protected set; }
    [field: SerializeField] public float CurrentHealth;
    
    private float _gravity = -9.8f * 2;
    #endregion


    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();


        RigidbodyCompo = GetComponent<Rigidbody>();
        ColliderCompo = GetComponent<CapsuleCollider>();

        BuffCompo = GetComponent<Buff>();

        PlayerStatData = Instantiate(PlayerStatData);
        PlayerStatData.SetOwner(this);
    }

    protected virtual void Start()
    {
        InitStairCheckPos();
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        RigidbodyCompo.velocity += new Vector3(0, _gravity, 0) * Time.deltaTime;

        MoveOnStair();
    }

    #region velocity control
    public void SetVelocity(Vector3 value)
    {
        RigidbodyCompo.velocity = value;
    }

    public void StopImmediately(bool withYAxis)
    {
        if (withYAxis)
        {
            RigidbodyCompo.velocity = Vector3.zero;
        }
        else
        {
            RigidbodyCompo.velocity = new Vector3(0, RigidbodyCompo.velocity.y);
        }
    }
    #endregion

    #region Collision Check logic
    public virtual bool IsGroundDetected() => Physics.Raycast(_groundChecker.position, Vector3.down, _groundCheckDistance * transform.localScale.y, _whatIsGround);
    #endregion

    #region delay coroutine logic
    public void StartDelayAction(float delayTime, Action todoAction)
    {
        StartCoroutine(DelayAction(delayTime, todoAction));
    }

    protected IEnumerator DelayAction(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
    #endregion

    #region stair logic
    private void InitStairCheckPos()
    {
        Vector3 upperCheckPos = _stairUpperChecker.localPosition;
        upperCheckPos.y = _stairHeight;

        _stairUpperChecker.localPosition = upperCheckPos;
    }

    public bool CheckStair(Vector3 dir)
    {
        Debug.DrawRay(_stairLowerChecker.position, transform.TransformDirection(dir) * _stairLowerCheckDistance, Color.red);
        Debug.DrawRay(_stairUpperChecker.position, transform.TransformDirection(dir) * _stairUpperCheckDistance, Color.red);

        RaycastHit hitLower;
        if (Physics.Raycast(_stairLowerChecker.position, transform.TransformDirection(dir), out hitLower, _stairLowerCheckDistance, _whatIsStair))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(_stairUpperChecker.position, transform.TransformDirection(dir), out hitUpper, _stairUpperCheckDistance, _whatIsStair))
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private void MoveOnStair()
    {
        if (CheckStair(Vector3.forward))
        {
            RigidbodyCompo.position -= new Vector3(0f, -_stairMoveSmooth, 0f);
        }

        if (CheckStair(new Vector3(1.5f, 0, 1)))
        {
            RigidbodyCompo.position -= new Vector3(0f, -_stairMoveSmooth, 0f);
        }
        
        if (CheckStair(new Vector3(-1.5f, 0, 1)))
        {
            RigidbodyCompo.position -= new Vector3(0f, -_stairMoveSmooth, 0f);
        }
    }
    #endregion
#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {      
        if (_groundChecker != null)
            Gizmos.DrawLine(_groundChecker.position, _groundChecker.position + new Vector3(0, -_groundCheckDistance, 0));
    }
#endif
}