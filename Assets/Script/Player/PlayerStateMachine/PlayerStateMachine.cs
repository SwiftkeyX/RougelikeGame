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

    // input
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private bool _isMovementPressed;
    private bool _isRunPressed;

    // _animator var
    private int _isMovingHash;

    // movement value
    private float _rotationPerFrame = 15f;
    private float _moveSpeedPerFrame = 4f;

    // state
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentState;

    // getter and setter
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public CharacterController CharacterController { get { return _characterController; } }
    public Animator Animator { get { return _animator; } }
    public PlayerStat Stat { get { return _stat; } }
    public float CurrentMovementX { get { return _currentMovement.x; } set { _currentMovement.x = value; } }
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float CurrentMovementZ { get { return _currentMovement.z; } set { _currentMovement.z = value; } }
    public float CurrentMovementInputX { get { return _currentMovementInput.x; } }
    public float CurrentMovementInputY { get { return _currentMovementInput.y; } }
    public bool IsMovementPressed { get { return _isMovementPressed; } }
    public bool IsRunPressed { get { return _isRunPressed; } }
    public int IsMovingHash { get { return _isMovingHash; } }


    void Awake()
    {
        // Initial dependency
        _playerInputAction = new PlayerInputAction();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _stat = GetComponent<PlayerStat>();

        // Initial State
        _factory = new PlayerStateFactory(this);
        _currentState = _factory.Grounded();
        _currentState.EnterState();

        // Initial _animator variable
        _isMovingHash = Animator.StringToHash("isMoving");

        // set the player input callback
        _playerInputAction.CharacterControl.Move.started += OnMovement;
        _playerInputAction.CharacterControl.Move.canceled += OnMovement;
        _playerInputAction.CharacterControl.Move.performed += OnMovement;
    }

    void Start()
    {

    }

    void Update()
    {
        HandleMove();
        HandleRotation();
        _currentState.UpdateStates();
        // Debug.Log("current State is" + _currentState);
    }

    private void HandleMove()
    {
        _characterController.Move(_currentMovement * _moveSpeedPerFrame * Time.deltaTime);
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

        if (_isMovementPressed) transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationPerFrame * Time.deltaTime);
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

}