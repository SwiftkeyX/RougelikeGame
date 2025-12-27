using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.SECOND;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, true);

    }
    public override void UpdateState()
    {
        CheckSwitchState();

        _ctx.Helper.UpdateAgentToPlayer();
    }
    public override void ExitState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, false);
        _ctx.Helper.StopAgent();
    }
    public override void CheckSwitchState()
    {
        if (!_ctx.DetectPlayer) SwitchState(_ctx.Factory.Observe());
    }
    public override void InitializeSubState()
    {
        SetSubState(_ctx.Factory.ForwardWalk());
    }
}
