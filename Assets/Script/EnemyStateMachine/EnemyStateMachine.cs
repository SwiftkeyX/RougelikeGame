using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

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

    // statemachine
    private EnemyBaseState _currentState;

    // helper
    private StateMachineHelper _helper;

    // movement
    private Vector3 _currentMovement;
    private float _speedPerFrame = 0.5f;
    private float _rotationPerFrame = 15f;

    // grounded state
    public float _minimalJumpTime = 0.5f; // this is GroundedBuffer (prevent state's change from Grounded to Jump every frame)
    private float _lastGroundedTime;

    // observe state
    private float _minimumObserveTime = 3f;

    // room var
    public bool _detectPlayer;
    public bool _alert;

    // animator var
    int _isWalkingHash;
    int _isForwardWalkingHash;
    int _isLeftWalkingHash;
    int _isRightWalkingHash;
    int _isJumpingHash;
    int _isAttackingHash;

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
    public int IsJumpingHash { get { return _isJumpingHash; } }
    public int isAttackingHash { get { return _isAttackingHash; } }
    public bool Alert { get { return _alert; } set { _alert = value; } }
    public bool DetectPlayer { get { return _detectPlayer; } }
    public float MinimalJumpTime { get { return _minimalJumpTime; } }
    public float LastGroundedTime { get { return _lastGroundedTime; } set { _lastGroundedTime = value; } }
    public float MinimumObserveTime { get { return _minimumObserveTime; } }


    void Awake()
    {
         // Initial dependency
        _brain = new Brain();
        _factory = new EnemyStateFactory(this);
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _helper = new StateMachineHelper(this);

        // get player transform
        _playerTransform = GameObject.FindWithTag("Player").transform;

        // Initial Animator var in Hash
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isForwardWalkingHash = Animator.StringToHash("isForwardWalking");
        _isLeftWalkingHash = Animator.StringToHash("isLeftWalking");
        _isRightWalkingHash = Animator.StringToHash("isRightWalking");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _isAttackingHash = Animator.StringToHash("isAttacking");
    }

    void Start()
    {
        // Initial state machine
        _currentState = _factory.Grounded();

        // close agent's movement (let ONLY characterController control the movement)
        // only agent purpose in this project is to PathFinding
        _agent.updatePosition = false;
        _agent.updateRotation = false;

        // test
        _detectPlayer = true;
    }

    void Update()
    {
        _characterController.Move(_currentMovement * _speedPerFrame * Time.deltaTime);
        HandleRotation();
        _currentState.UpdateStates();

        // test  
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

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

        // if (_isMovementPressed) 
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationPerFrame * Time.deltaTime);
    }
}