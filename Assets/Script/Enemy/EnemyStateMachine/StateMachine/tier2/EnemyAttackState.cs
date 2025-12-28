using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.SECOND;
    }

    public override void EnterState()
    {
        _ctx.Animator.SetTrigger(_ctx.IsAttackHash);
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        _ctx.AttackEnd = false; // reset attack
    }
    public override void CheckSwitchState()
    {
        if (_ctx.AttackEnd) SwitchState(_ctx.Factory.Cooldown());
    }
    public override void InitializeSubState() { }
}
