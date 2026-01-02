using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory)
    {
        _isRoot = false;
    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, true);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, false);
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

        if (_ctx.IsMovementPressed && _ctx.IsRunPressed) SwitchState(_factory.Run());
    }
    public override void InitializeSubState() { }
}
