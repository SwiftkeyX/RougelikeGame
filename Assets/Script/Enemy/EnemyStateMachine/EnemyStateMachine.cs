using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Hold the context for overall enemy's statemachine  
/// </summary>
public class EnemyStateMachine : MonoBehaviour
{
    // dependency
    private CharacterController _characterController;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private EnemyStateFactory _factory;
    private StateMachineHelper _helper;
    private GizmoContext _gizmoContext;
    private Weapon _weapon;
    [SerializeField] private EnemyStat _stat;

    // statemachine
    private EnemyBaseState _currentState;

    // unity's layerMask
    private LayerMask _groundLayer;

    // agent var
    private bool _isAgentStop;

    // movement
    private Vector3 _currentMovement;

    // grounded state
    public float _minimalJumpTime = 0.5f; // this is GroundedBuffer (prevent state's change from Grounded to Jump every frame)
    private float _lastGroundedTime;
    private Vector3 _sphereOrigin;
    private float _groundedSphereRadius;
    private float _groundCheckDistance;
    private float _groundCheckStartOffset;

    // room var
    public bool _detectPlayer;
    public bool _alert;

    // animator var
    int _isWalkingHash;
    int _isForwardWalkingHash;
    int _isLeftWalkingHash;
    int _isRightWalkingHash;
    int _isJumpHash;
    int _isAttackHash;
    int _attackID;
    int _isAttackEndHash;

    // ==================================== getter and setter ===============================================
    // dependency
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public EnemyStateFactory Factory { get { return _factory; } }
    public Animator Animator { get { return _animator; } }
    public StateMachineHelper Helper { get { return _helper; } }
    public EnemyStat EnemyStat { get { return _stat; } }
    public Weapon Weapon { get { return _weapon; } }
    // agent
    public void AgentSetDes(Vector3 pos) { _agent.SetDestination(pos); }
    public Vector3 CalculatePath() { return _agent.desiredVelocity; }
    public void AgentUpdateCurrentPosition() { _agent.nextPosition = transform.position; }
    public bool IsAgentStop { get { return _isAgentStop; } set { _isAgentStop = value; } } // flag to tell that agent is stop or not
    // movement
    public Vector3 PlayerPosition { get { return _playerTransform.position; } }
    public Vector3 CurrentMovement { get { return _currentMovement; } set { _currentMovement = value; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    // animator hash
    public int IsWalkingHash { get { return _isWalkingHash; } }
    public int IsForwardWalkingHash { get { return _isForwardWalkingHash; } }
    public int IsLeftWalkingHash { get { return _isLeftWalkingHash; } }
    public int IsRightWalkingHash { get { return _isRightWalkingHash; } }
    public int IsJumpHash { get { return _isJumpHash; } }
    public int IsAttackHash { get { return _isAttackHash; } }
    public int AttackID { get { return _attackID; } }
    public int IsAttackEndHash { get { return _isAttackEndHash; } }
    // enemy's condition var
    public bool Alert { get { return _alert; } set { _alert = value; } }
    public bool DetectPlayer { get { return _detectPlayer; } }
    // jump state
    public float MinimalJumpTime { get { return _minimalJumpTime; } }
    public float LastGroundedTime { get { return _lastGroundedTime; } set { _lastGroundedTime = value; } }
    // ground state
    public LayerMask GroundLayer { get { return _groundLayer; } }
    public Vector3 SphereOrigin { get { return _sphereOrigin; } set { _sphereOrigin = value; } }
    public float GroundCheckStartOffset { get { return _groundCheckStartOffset; } }
    public float GroundedSphereRadius { get { return _groundedSphereRadius; } }
    public float GroundCheckDistance { get { return _groundCheckDistance; } }

    void Awake()
    {
        // Initial dependency
        _factory = new EnemyStateFactory(this);
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _helper = new StateMachineHelper(this);
        _gizmoContext = GetComponent<GizmoContext>();
        _weapon = GetComponent<Weapon>();

        // get player transform
        _playerTransform = GameObject.FindWithTag("Player").transform;

        // Initial Animator var in Hash
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isForwardWalkingHash = Animator.StringToHash("isForwardWalking");
        _isLeftWalkingHash = Animator.StringToHash("isLeftWalking");
        _isRightWalkingHash = Animator.StringToHash("isRightWalking");
        _isJumpHash = Animator.StringToHash("isJump");
        _isAttackHash = Animator.StringToHash("isAttack");
        _attackID = Animator.StringToHash("attackID");
        _isAttackEndHash = Animator.StringToHash("isAttackEnd");

        // layermask
        _groundLayer = LayerMask.GetMask("Ground");

        // Initial Ground Check var
        InitialGroundedCheck();
    }

    private void InitialGroundedCheck()
    {
        _groundCheckDistance = 0.2f;
        _groundCheckStartOffset = 0.1f;
        _groundedSphereRadius = _characterController.radius;
        _sphereOrigin = transform.position + Vector3.up * _groundCheckStartOffset;
    }

    void Start()
    {
        // Initial state machine
        _currentState = _factory.Initial();

        /// <summary>
        /// close agent's movement (let ONLY characterController control the movement) 
        /// only agent purpose in this project is to PathFinding
        /// </summary>
        _agent.updatePosition = false;
        _agent.updateRotation = false;

        // test
        _detectPlayer = true;
    }

    void Update()
    {
        _characterController.Move(_currentMovement * _stat.SpeedPerFrame * Time.deltaTime);
        HandleRotation();
        _currentState.UpdateStates();

        // // test  
        Debug.Log(
            "FirstState: " + _currentState +
            " SecondState: " + (_currentState?.CurrentSubState?.ToString() ?? "null") +
            " ThirdState: " + (_currentState?.CurrentSubState?.CurrentSubState?.ToString() ?? "null")
        );
        // Debug.Log("CurrentMovementY: " + CurrentMovementY);
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = _currentMovement.z;

        // LookRotation() can't receive zero vector, so we validate it first 
        if (positionToLookAt.sqrMagnitude < 0.0001f) return;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

        // if (_isMovementPressed) 
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _stat.RotationPerFrame * Time.deltaTime);
    }

}