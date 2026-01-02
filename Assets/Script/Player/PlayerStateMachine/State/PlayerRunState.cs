using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory)
    {
        _isRoot = false;
    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, true);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, true);
    }
    public override void UpdateState()
    {
        CheckSwitchState();
        _ctx.CurrentMovementX = _ctx.CurrentMovementInputX;
        _ctx.CurrentMovementZ = _ctx.CurrentMovementInputY;
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (!_ctx.IsMovementPressed) SwitchState(_factory.Idle());

        if (_ctx.IsMovementPressed && !_ctx.IsRunPressed) SwitchState(_factory.Walk());
    }
    public override void InitializeSubState() { }
}
