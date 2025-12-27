using UnityEngine;

public class EnemyForwardWalkState : EnemyBaseState
{
    public EnemyForwardWalkState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.THIRD;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, true);
        _ctx.Animator.SetBool(_ctx.IsForwardWalkingHash, true);
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, false);
        _ctx.Animator.SetBool(_ctx.IsForwardWalkingHash, false);
    }
    public override void CheckSwitchState() { }
    public override void InitializeSubState()
    {

    }
}
