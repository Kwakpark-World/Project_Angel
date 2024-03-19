using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ground Checker")]
    [SerializeField] protected Transform _groundChecker;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected LayerMask _whatIsGround;

    [Header("Stair Checker")]
    [SerializeField] protected Transform _stairUpperChecker;
    [SerializeField] protected Transform _stairLowerChecker;
    [SerializeField] protected float _stairUpperCheckDistance;
    [SerializeField] protected float _stairLowerCheckDistance;

    [Header("Stair Parameters")]
    [SerializeField] private float _stairHeight;
    [SerializeField] private float _stairMoveSmooth;

    #region components
    public Animator AnimatorCompo { get; private set; }
    public Rigidbody RigidbodyCompo { get; private set; }
    public CapsuleCollider ColliderCompo { get; private set; }

    public DefaultCollider DefaultCollider { get; private set; }

    [field: SerializeField] public CharacterStat CharStat { get; private set; }

    [SerializeField] protected float _gravity = -9.8f;
    #endregion

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        RigidbodyCompo = GetComponent<Rigidbody>();
        ColliderCompo = GetComponent<CapsuleCollider>();

        CharStat = Instantiate(CharStat);
        CharStat.SetOwner(this);
    }

    protected virtual void Start()
    {
        InitCollider();

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

    #region collider control
    public void SetCollider(float radius = 0.1456659f, float height = 1.794054f)
    {
        ColliderCompo.radius = radius;
        ColliderCompo.height = height;
    }

    public void SetAnimCollider(float limitValue, float changeValue, float delay)
    {
        StartCoroutine(ColliderChange(limitValue, changeValue, delay));
    }

    private IEnumerator ColliderChange(float limitValue, float changeValue, float delay)
    {
        if (changeValue > 0)
        {
            while (ColliderCompo.height > limitValue)
            {
                SetCollider(DefaultCollider.radius, ColliderCompo.height - changeValue);
                yield return new WaitForSeconds(delay);
            }
        }
        else
        {
            while (ColliderCompo.height < limitValue)
            {
                SetCollider(DefaultCollider.radius, ColliderCompo.height - changeValue);
                yield return new WaitForSeconds(delay);
            }
        }

        yield return null;
    }

    private void InitCollider()
    {
        ColliderCompo.center = new Vector3(0f, 0.95f, 0f);
        ColliderCompo.radius = 0.1456659f;
        ColliderCompo.height = 1.794054f;

        DefaultCollider = new DefaultCollider(ColliderCompo.center, ColliderCompo.radius, ColliderCompo.height);
    }
    #endregion

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
    public virtual bool IsGroundDetected() => Physics.Raycast(_groundChecker.position, Vector3.down, _groundCheckDistance, _whatIsGround);
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
        Vector3 upperCheckPos = _stairUpperChecker.position;
        upperCheckPos.y = _stairHeight;

        _stairUpperChecker.position = upperCheckPos;
    }

    public bool CheckStair(Vector3 dir)
    {
        RaycastHit hitLower;
        if (Physics.Raycast(_stairLowerChecker.position, transform.TransformDirection(dir), out hitLower, _stairLowerCheckDistance))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(_stairUpperChecker.position, transform.TransformDirection(dir), out hitUpper, _stairUpperCheckDistance))
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

        //RaycastHit hitLower;
        //if (Physics.Raycast(_stairLowerChecker.position, transform.TransformDirection(Vector3.forward), out hitLower, _stairLowerCheckDistance))
        //{
        //    RaycastHit hitUpper;
        //    if (!Physics.Raycast(_stairUpperChecker.position, transform.TransformDirection(Vector3.forward), out hitUpper, _stairUpperCheckDistance))
        //    {
        //        RigidbodyCompo.position -= new Vector3(0f, -_stairMoveSmooth, 0f);
        //    }
        //}

        //RaycastHit hitLower45;
        //if (Physics.Raycast(_stairLowerChecker.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, _stairLowerCheckDistance))
        //{
        //    RaycastHit hitUpper45;
        //    if (!Physics.Raycast(_stairUpperChecker.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, _stairUpperCheckDistance))
        //    {
        //        RigidbodyCompo.position -= new Vector3(0f, -_stairMoveSmooth, 0f);
        //    }
        //}

        //RaycastHit hitLowerMinus45;
        //if (Physics.Raycast(_stairLowerChecker.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, _stairLowerCheckDistance))
        //{
        //    RaycastHit hitUpperMinus45;
        //    if (!Physics.Raycast(_stairUpperChecker.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, _stairUpperCheckDistance))
        //    {
        //        RigidbodyCompo.position -= new Vector3(0f, -_stairMoveSmooth, 0f);
        //    }
        //}
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

public struct DefaultCollider 
{
    public Vector3 center;
    public float radius;
    public float height;

    public DefaultCollider(Vector3 center, float radius, float height)
    {
        this.center = center;
        this.radius = radius;
        this.height = height;
    }

}
