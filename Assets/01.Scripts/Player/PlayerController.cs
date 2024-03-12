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
    [SerializeField] protected Transform _stairChecker;
    [SerializeField] protected float _stairCheckDistance;
    [SerializeField] protected LayerMask _whatIsStair;

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
        ColliderCompo.center = new Vector3(0f, 0.95f, 0f);
        ColliderCompo.radius = 0.1456659f;
        ColliderCompo.height = 1.794054f;

        DefaultCollider = new DefaultCollider(ColliderCompo.center, ColliderCompo.radius, ColliderCompo.height);
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
        RigidbodyCompo.velocity += new Vector3(0, _gravity, 0) * Time.deltaTime;
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
        while (ColliderCompo.height > limitValue)
        {
            SetCollider(default, ColliderCompo.height - changeValue);
            yield return new WaitForSeconds(delay);
        }

        yield return null;
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

    #region Collision Check loginc
    public virtual bool IsGroundDetected() => Physics.Raycast(_groundChecker.position, Vector3.down, _groundCheckDistance, _whatIsGround);
    public virtual bool IsStairCheck() => Physics.Raycast(_stairChecker.position, Vector3.forward, _stairCheckDistance, _whatIsStair);
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


#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (IsStairCheck())
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.white;
        
        if (_groundChecker != null)
            Gizmos.DrawLine(_groundChecker.position, _groundChecker.position + new Vector3(0, -_groundCheckDistance, 0));
        if (_stairChecker != null)
            Gizmos.DrawLine(_stairChecker.position, _stairChecker.position + new Vector3(0, 0, _stairCheckDistance));

        
        
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
