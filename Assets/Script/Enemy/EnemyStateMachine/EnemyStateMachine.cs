using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Hold the context for overall enemy's statemachine  
/// </summary>
public class EnemyStateMachine : MonoBehaviour
{
    // dependency
    private Brain _brain;
    private CharacterController _characterController;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private EnemyStateFactory _factory;
    private StateMachineHelper _helper;
    private EnemyStat _stat;

    // statemachine
    private EnemyBaseState _currentState;

    // agent var
    private bool _isAgentStop;

    // movement
    private Vector3 _currentMovement;
    
    // grounded state
    public float _minimalJumpTime = 0.5f; // this is GroundedBuffer (prevent state's change from Grounded to Jump every frame)
    private float _lastGroundedTime;

    // combat var
    private bool _attackEnd;

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

    // getter and setter
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public NavMeshAgent Agent { get { return _agent; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public EnemyStateFactory Factory { get { return _factory; } }
    public Animator Animator { get { return _animator; } }
    public StateMachineHelper Helper { get { return _helper; } }
    public void AgentSetDes(Vector3 pos) { _agent.SetDestination(pos); }
    public Vector3 CalculatePath() { return _agent.desiredVelocity; }
    public void AgentUpdateCurrentPosition() { _agent.nextPosition = transform.position; }
    public Vector3 PlayerPosition { get { return _playerTransform.position; } }
    public Vector3 CurrentMovement { get { return _currentMovement; } set { _currentMovement = value; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public int IsWalkingHash { get { return _isWalkingHash; } }
    public int IsForwardWalkingHash { get { return _isForwardWalkingHash; } }
    public int IsLeftWalkingHash { get { return _isLeftWalkingHash; } }
    public int IsRightWalkingHash { get { return _isRightWalkingHash; } }
    public int IsJumpHash { get { return _isJumpHash; } }
    public int IsAttackHash { get { return _isAttackHash; } }
    public bool Alert { get { return _alert; } set { _alert = value; } }
    public bool DetectPlayer { get { return _detectPlayer; } }
    public float MinimalJumpTime { get { return _minimalJumpTime; } }
    public float LastGroundedTime { get { return _lastGroundedTime; } set { _lastGroundedTime = value; } }
    public float AttackRange { get { return _stat.AttackRange; } }
    public bool AttackEnd { get { return _attackEnd; } set { _attackEnd = value; } }
    public bool IsAgentStop { get { return _isAgentStop; } set { _isAgentStop = value; } }


    void Awake()
    {
        // Initial dependency
        _brain = new Brain();
        _factory = new EnemyStateFactory(this);
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _helper = new StateMachineHelper(this);
        _stat = GetComponent<EnemyStat>();

        // get player transform
        _playerTransform = GameObject.FindWithTag("Player").transform;

        // Initial Animator var in Hash
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isForwardWalkingHash = Animator.StringToHash("isForwardWalking");
        _isLeftWalkingHash = Animator.StringToHash("isLeftWalking");
        _isRightWalkingHash = Animator.StringToHash("isRightWalking");
        _isJumpHash = Animator.StringToHash("isJump");
        _isAttackHash = Animator.StringToHash("isAttack");

    }

    void Start()
    {
        // Initial state machine
        _currentState = _factory.Initial();

        // close agent's movement (let ONLY characterController control the movement)
        // only agent purpose in this project is to PathFinding
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
        // Debug.Log(
        //     "FirstState: " + _currentState +
        //     " SecondState: " + (_currentState?.CurrentSubState?.ToString() ?? "null") +
        //     " ThirdState: " + (_currentState?.CurrentSubState?.CurrentSubState?.ToString() ?? "null")
        // );
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

    // Event Handler (Behaviour script attach to Attack State)
    public void OnAttackStart() => _attackEnd = false;
    public void OnAttackFinish() => _attackEnd = true;
}