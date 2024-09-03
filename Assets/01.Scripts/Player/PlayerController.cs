using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    [Header("Ground Checker")]
    [SerializeField] protected Transform _groundChecker;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public LayerMask whatIsStair;
    [SerializeField] public LayerMask whatIsWall;
    [SerializeField] public float groundCheckDistanceTolerance;
    [SerializeField] public float playerCenterToGroundDistance = 0.0f;
    public RaycastHit groundCheckHit = new RaycastHit();

    #region components
    [field: Space(30f), Header("Components")]
    public Animator AnimatorCompo { get; set; }
    public Rigidbody RigidbodyCompo { get; private set; }
    public CapsuleCollider ColliderCompo { get; private set; }
    public Buff BuffCompo { get; private set; }
    [field: SerializeField]
    public PlayerStat PlayerStatData { get; protected set; }
    #endregion

    public Transform playerCenter;

    private float _currentHealth;
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }

        set
        {
            _currentHealth = Mathf.Clamp(value, 0f, PlayerStatData.GetMaxHealth());
        }
    }
    private float _gravity = -9.8f;

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

    #region Collision Check logic
    public virtual bool IsGroundDetected()
    {
        if (_groundChecker == null)
            _groundChecker = transform.Find("GroundChecker");

        bool groundCheck = Physics.Raycast(_groundChecker.position, Vector3.down, out groundCheckHit, groundCheckDistanceTolerance * transform.localScale.y, whatIsGround);


        playerCenterToGroundDistance = Vector3.Distance(groundCheckHit.point, playerCenter.position);

        return groundCheck;
    }
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
            Gizmos.DrawLine(_groundChecker.position, _groundChecker.position + new Vector3(0, -groundCheckDistanceTolerance, 0));
    }
#endif
}