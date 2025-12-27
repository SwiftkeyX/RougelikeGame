using UnityEngine;

public class EnemyObserveState : EnemyBaseState
{
    private float _onEnterObserveTime;
    private float _onExitObserveTime;
    private float _minimumObserveTime = 3f;

    public EnemyObserveState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.SECOND;
        InitializeSubState();
    }

    public override void EnterState()
    {
        _onEnterObserveTime = Time.time;
    }
    public override void UpdateState()
    {
        CheckSwitchState();

        _ctx.Helper.UpdateAgentToPlayer();
    }
    public override void ExitState()
    {
        _ctx.Helper.StopAgent();
    }
    public override void CheckSwitchState()
    {
        if (_ctx.DetectPlayer) SwitchState(_ctx.Factory.Chase());
    }
    public override void InitializeSubState()
    {
        // 70% go strafe
        if (Random.Range(0f, 1f) < 0.7f) SetSubState(_ctx.Factory.Strafe());

        // 30% go Idle
        else SetSubState(_ctx.Factory.Idle());
    }

    // // Call by its substate to not let state constantly switch between Strafe/Idle every frame
    // public bool ObserveExitTime()
    // {
    //     _onExitObserveTime = Time.time;

    //     if (_onExitObserveTime - _onEnterObserveTime > _minimumObserveTime) return true;

    //     return false;
    // }
}
