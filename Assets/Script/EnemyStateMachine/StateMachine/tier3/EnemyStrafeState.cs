using UnityEngine;

public class EnemyStrafeState : EnemyBaseState
{
    public EnemyStrafeState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.THIRD;
    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, true);

        // 50/50 to go left strafe or right strafe
        if (Random.Range(0, 1f) > 0.5f) _ctx.Animator.SetBool(_ctx.IsLeftWalkingHash, true);

        else _ctx.Animator.SetBool(_ctx.IsRightWalkingHash, true);
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, false);
        _ctx.Animator.SetBool(_ctx.IsLeftWalkingHash, false);
        _ctx.Animator.SetBool(_ctx.IsRightWalkingHash, false);
    }
    public override void CheckSwitchState()
    {

    }
    public override void InitializeSubState() { }
}
