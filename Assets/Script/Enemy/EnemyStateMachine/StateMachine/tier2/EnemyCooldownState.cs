using UnityEngine;

public class EnemyCooldownState : EnemyBaseState
{
    private float _enterCooldownTime;
    private float _minimumCooldownTime = 3f;

    public EnemyCooldownState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.SECOND;
    }

    public override void EnterState()
    {
        _enterCooldownTime = Time.time;
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        float currentTime = Time.time;

        // wait for cooldown time first
        if (currentTime - _enterCooldownTime >= _minimumCooldownTime)
        {
            if (_ctx.DetectPlayer && _ctx.Helper.DesignWhichAttackToUse()) SwitchState(_ctx.Factory.Attack());

            else if (_ctx.DetectPlayer && !_ctx.Helper.PlayerInRange()) SwitchState(_ctx.Factory.Chase());

            else if (_ctx.DetectPlayer) SwitchState(_ctx.Factory.Observe());
        }
    }
    public override void InitializeSubState()
    {
        SetSubState(_ctx.Factory.Idle());
    }
}
