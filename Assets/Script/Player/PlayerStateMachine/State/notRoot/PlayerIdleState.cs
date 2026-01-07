using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory)
    {
        _isRoot = false;
    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsMovingHash, false);
    }
    public override void UpdateState()
    {
        CheckSwitchState();
        _ctx.CurrentMovementX = 0f;
        _ctx.CurrentMovementZ = 0f;
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (_ctx.IsMovementPressed) SwitchState(_factory.Move());

        else if (_ctx.IsAttackPressed) SwitchState(_factory.Attack());

        else if (_ctx.IsRollPressed) SwitchState(_factory.Roll());
    }
    public override void InitializeSubState() { }
}
