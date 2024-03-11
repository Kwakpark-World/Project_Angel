using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("collision checker")]
    [SerializeField] protected Transform _groundChecker;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected LayerMask _whatIsGround;

    #region components
    public Animator AnimatorCompo { get; private set; }
    public Rigidbody RigidbodyCompo { get; private set; }
    public Collider ColliderCompo { get; private set; }

    [field: SerializeField] public CharacterStat CharStat { get; private set; }

    [SerializeField] protected float _gravity = -9.8f;
    #endregion

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        RigidbodyCompo = GetComponent<Rigidbody>();
        ColliderCompo = visualTrm.GetComponent<Collider>();

        CharStat = Instantiate(CharStat);
        CharStat.SetOwner(this);
    }

    protected virtual void Start()
    {
        ColliderCompo.GetComponent<CapsuleCollider>().radius = 0.1456659f;
        ColliderCompo.GetComponent<CapsuleCollider>().height = 1.794054f;
        ColliderCompo.GetComponent<CapsuleCollider>().center = new Vector3(0, 0.95f, 0);
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
        RigidbodyCompo.velocity += new Vector3(0, _gravity, 0) * Time.deltaTime;
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

    #region Collision Check loginc
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


#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (_groundChecker != null)
            Gizmos.DrawLine(_groundChecker.position, _groundChecker.position + new Vector3(0, -_groundCheckDistance, 0));
    }
#endif
}