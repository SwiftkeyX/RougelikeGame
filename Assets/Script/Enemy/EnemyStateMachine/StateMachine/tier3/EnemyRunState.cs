public class EnemyRunState : EnemyBaseState
{
    public EnemyRunState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory) { _order = Order.THIRD; }

    public override void EnterState() { }
    public override void UpdateState() { }
    public override void ExitState() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}
