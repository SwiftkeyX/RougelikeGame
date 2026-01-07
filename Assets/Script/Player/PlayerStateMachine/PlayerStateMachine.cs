using UnityEngine;
using UnityEngine.InputSystem;


// Context for all the state class 
public class PlayerStateMachine : MonoBehaviour
{
    // dependency
    private PlayerInputAction _playerInputAction;
    private CharacterController _characterController;
    private Animator _animator;
    private PlayerStat _stat;
    private Weapon _weapon;
    private BufferInput _bufferInput;

    // input
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private bool _isMovementPressed;
    private bool _isRollPressed;
    private bool _isAttackPressed;

    // _animator var
    private int _isMovingHash;
    private int _attackIDHash;
    private int _isAttackTriggerHash;
    private int _isAttackEndHash;
    private int _isRollTriggerHash;

    // movement value
    private float _rotationPerFrame = 15f;
    private float _rollRotationSpeedPerFrame = 1000f;
    private float _moveSpeedPerFrame = 4f;
    private bool _movementLock;
    private bool _isRollBuffer;
    private bool _isRolling;
    private Quaternion _rollRotation;

    // attack var
    private bool _allowAttackBuffer;

    // state
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentState;

    // =============================== getter and setter ======================================
    // dependency
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public Animator Animator { get { return _animator; } }
    public PlayerStat Stat { get { return _stat; } }
    public Weapon Weapon { get { return _weapon; } }
    public BufferInput BufferInput { get { return _bufferInput; } }
    // movement
    public float CurrentMovementX { get { return _currentMovement.x; } set { _currentMovement.x = value; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float CurrentMovementZ { get { return _currentMovement.z; } set { _currentMovement.z = value; } }
    public float CurrentMovementInputX { get { return _currentMovementInput.x; } }
    public float CurrentMovementInputY { get { return _currentMovementInput.y; } }
    public bool MovementLock { get { return _movementLock; } set { _movementLock = value; } }
    public bool IsRollBuffer { get { return _isRollBuffer; } set { _isRollBuffer = value; } }
    public bool IsRolling { get { return _isRolling; } set { _isRolling = value; } }
    public Quaternion RollRotation { get { return _rollRotation; } set { _rollRotation = value; } }
    // input
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsAttackPressed { get { return _isAttackPressed; } set { _isAttackPressed = value; } }
    public bool IsRollPressed { get { return _isRollPressed; } set { _isRollPressed = value; } }
    // animation hash
    public int IsMovingHash { get { return _isMovingHash; } }
    public int AttackIDHash { get { return _attackIDHash; } }
    public int IsAttackTriggerHash { get { return _isAttackTriggerHash; } }
    public int IsAttackEndHash { get { return _isAttackEndHash; } }
    public int IsRollTriggerHash { get { return _isRollTriggerHash; } }
    // animation variable's value getter
    public int AttackIDValue { get { return _animator.GetInteger("AttackID"); } }
    // combat var
    public bool AllowAttackBuffer { get { return _allowAttackBuffer; } set { _allowAttackBuffer = value; } }

    void Awake()
    {
        // Initial dependency
        _playerInputAction = new PlayerInputAction();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _stat = GetComponent<PlayerStat>();
        _weapon = GetComponent<Weapon>();
        _bufferInput = new BufferInput(this);

        // Initial State
        _factory = new PlayerStateFactory(this);
        _currentState = _factory.Grounded();
        _currentState.EnterState();

        // Initial _animator variable
        _isMovingHash = Animator.StringToHash("isMoving");
        _attackIDHash = Animator.StringToHash("AttackID");
        _isAttackTriggerHash = Animator.StringToHash("isAttackTrigger");
        _isAttackEndHash = Animator.StringToHash("isAttackEnd");
        _isRollTriggerHash = Animator.StringToHash("isRollTrigger");

        // set the player input callback
        _playerInputAction.CharacterControl.Move.started += OnMovement;
        _playerInputAction.CharacterControl.Move.canceled += OnMovement;
        _playerInputAction.CharacterControl.Move.performed += OnMovement;
        _playerInputAction.CharacterControl.Roll.performed += OnRoll;
        _playerInputAction.CharacterControl.Attack.performed += OnAttack;
    }

    void Start()
    {
        _animator.SetBool(IsAttackEndHash, false);
    }

    void Update()
    {
        HandleMove();
        HandleRotation();
        _currentState.UpdateStates();
        // Debug.Log("current State is: " + _currentState + " sub-state is: " + _currentState.CurrentSubState);
    }

    private void HandleMove()
    {
        // movement while attacking (0.1f is to make CC be able to use Move() correctly)
        // if we don't use Move() correctly, the CC's groundCheck won't work
        if (_movementLock) _characterController.Move(_currentMovement * 0.1f * Time.deltaTime);

        // let root motion move player while rolling (make CC affect player's movement very little while rolling)
        else if (_isRolling) _characterController.Move(_currentMovement * 0.1f * Time.deltaTime);

        // walk normally
        else _characterController.Move(_currentMovement * _moveSpeedPerFrame * Time.deltaTime);
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

        // rotate to playerInput while moving normally
        if (_isMovementPressed && !_isRolling) { transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationPerFrame * Time.deltaTime); }
        
        // rotate to playerInput when rolling
        else if (_isRolling) { transform.rotation = Quaternion.Slerp(currentRotation, _rollRotation, _rollRotationSpeedPerFrame * Time.deltaTime); }
    }

    void OnEnable()
    {
        _playerInputAction.Enable();
    }

    void OnDisable()
    {
        _playerInputAction.Disable();
    }

    // ---------------------------- Callback function for Input button -----------------------------------
    private void OnMovement(InputAction.CallbackContext ctx)
    {
        _currentMovementInput = ctx.ReadValue<Vector2>();

        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    private void OnRoll(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        _isRollPressed = true;
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        _isAttackPressed = true;
    }

}