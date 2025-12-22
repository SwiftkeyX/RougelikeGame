using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    // dependency
    private PlayerInputAction playerInputAction;
    private CharacterController characterController;
    private Animator animator;

    // input
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;

    // animator var
    private bool isMovementPressed;
    private bool isRunPressed;
    private int isWalkingHash;
    private int isRunningHash;

    // movement value
    private float rotationPerFrame = 15f;
    private float walkSpeedPerFrame = 2f;
    private float runSpeedPerFrame = 4.5f;

    // void Awake()
    // {
    //     // Initial dependency
    //     playerInputAction = new PlayerInputAction();
    //     characterController = GetComponent<CharacterController>();
    //     animator = GetComponent<Animator>();

    //     // Initial animator variable
    //     isWalkingHash = Animator.StringToHash("isWalking");
    //     isRunningHash = Animator.StringToHash("isRunning");
    // }

    // void Start()
    // {
    //     // Input logic
    //     playerInputAction.CharacterControl.Move.started += OnMovement;
    //     playerInputAction.CharacterControl.Move.canceled += OnMovement;
    //     playerInputAction.CharacterControl.Move.performed += OnMovement;
    //     playerInputAction.CharacterControl.Run.started += OnRun;
    //     playerInputAction.CharacterControl.Run.canceled += OnRun;

    // }

    // void Update()
    // {
    //     HandleMove();
    //     HandleRotation();
    //     HandleAnimation();
    //     HandleGravity();
    // }

    // void OnEnable()
    // {
    //     playerInputAction.Enable();
    // }

    // void OnDisable()
    // {
    //     playerInputAction.Disable();
    // }

    // // ---------------------------- Callback function for Input button -----------------------------------
    // private void OnMovement(InputAction.CallbackContext ctx)
    // {
    //     currentMovementInput = ctx.ReadValue<Vector2>();
    //     currentMovement.x = currentMovementInput.x;
    //     currentMovement.z = currentMovementInput.y;

    //     isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
    // }

    // private void OnRun(InputAction.CallbackContext ctx)
    // {
    //     isRunPressed = ctx.ReadValueAsButton();
    // }

    // ---------------------------------------- Main function -----------------------------------------------
    // private void HandleMove()
    // {
    //     if (isRunPressed) characterController.Move(currentMovement * runSpeedPerFrame * Time.deltaTime);
    //     else characterController.Move(currentMovement * walkSpeedPerFrame * Time.deltaTime);
    // }

    private void HandleAnimation()
    {
        // run
        if (isMovementPressed && isRunPressed)
        {
            animator.SetBool(isRunningHash, true);
        }
        else
        {
            animator.SetBool(isRunningHash, false);
        }

        // walk
        if (isMovementPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else
        {
            animator.SetBool(isWalkingHash, false);
        }
    }

    // private void HandleRotation()
    // {
    //     Vector3 positionToLookAt;
    //     positionToLookAt.x = currentMovement.x;
    //     positionToLookAt.y = 0;
    //     positionToLookAt.z = currentMovement.z;

    //     Quaternion currentRotation = transform.rotation;
    //     Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

    //     if (isMovementPressed) transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationPerFrame * Time.deltaTime);
    // }

    private void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundValue = 0f;
            currentMovement.y = groundValue;
        }
        else
        {
            float gravity = 9.8f;
            currentMovement.y -= gravity;
        }
    }
}
