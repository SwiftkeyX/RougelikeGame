using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory)
    {
        _isRoot = false;
    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsMovingHash, true);
    }
    public override void UpdateState()
    {
        CheckSwitchState();
        _ctx.CurrentMovementX = _ctx.CurrentMovementInputX;
        _ctx.CurrentMovementZ = _ctx.CurrentMovementInputY;
    }
    public override void ExitState()
    {
        _ctx.Animator.SetBool(_ctx.IsMovingHash, false);
    }
    public override void CheckSwitchState()
    {
        if (!_ctx.IsMovementPressed) SwitchState(_factory.Idle());
    }
    public override void InitializeSubState() { }
}
