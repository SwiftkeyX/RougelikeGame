using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.THIRD;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, false);

        // reset agent (not let agent track player when in Idle)
        _ctx.Helper.StopAgent();
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        // _currentSuperState.ObserveExitTime();
    }
    public override void InitializeSubState() { }
}
