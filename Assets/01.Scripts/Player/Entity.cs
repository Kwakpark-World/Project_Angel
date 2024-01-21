using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("collision checker")]
    [SerializeField] protected Transform _groundChecker;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected LayerMask _whatIsGround;

    #region components
    public Animator AnimatorCompo { get; private set; }
    public Rigidbody RigidbodyCompo { get; private set; }

    [field: SerializeField] public CharacterStat CharStat { get; private set; }

    [SerializeField] protected float _gravity = -9.8f;
    #endregion
    //
    #region facing
    public int FacingDirection { get; private set; } = 1; // ¿À¸¥ÂÊ 1, ¿ÞÂÊ -1
    #endregion 

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        RigidbodyCompo = GetComponent<Rigidbody>();

        CharStat = Instantiate(CharStat);
        CharStat.SetOwner(this);
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
        RigidbodyCompo.velocity += new Vector3(0, _gravity, 0) * Time.deltaTime;
    }

    #region flip control
    public virtual void Flip()
    {
        FacingDirection = FacingDirection * -1;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float x)
    {
        bool goToRight = x > 0 && FacingDirection < 0;
        bool goToLeft = x < 0 && FacingDirection > 0;

        if (goToRight || goToLeft)
        {
            Flip();
        }

    }
    #endregion

    #region velocity control
    public void SetVelocity(float x, float y, float z, bool doNotFlip = false)
    {
        RigidbodyCompo.velocity = new Vector3(x, y, z);
        if (!doNotFlip)
        {
            FlipController(x);
        }
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