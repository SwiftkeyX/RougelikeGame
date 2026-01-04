using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory)
    {
        InitializeSubState();
        _isRoot = true;
    }

    public override void EnterState()
    {
        float groundedValue = 0f;
        _ctx.CurrentMovementY = groundedValue;
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (!_ctx.CharacterController.isGrounded)
        {
            SwitchState(_factory.Jump());
        }
    }
    public override void InitializeSubState()
    {
        if (!_ctx.IsMovementPressed) SetSubState(_factory.Idle());

        if (_ctx.IsMovementPressed) SetSubState(_factory.Move());
    }
}
