using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.SECOND;
    }

    public override void EnterState()
    {
    }
    public override void UpdateState()
    {
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
    }
    public override void InitializeSubState() { }
}
