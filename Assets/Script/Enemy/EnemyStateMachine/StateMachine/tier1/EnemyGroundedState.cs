using UnityEngine;

public class EnemyGroundedState : EnemyBaseState
{
    public EnemyGroundedState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.FIRST;
    }

    public override void EnterState()
    {
        float groundedValue = 0f;
        _ctx.CurrentMovementY = groundedValue;
    }
    public override void UpdateState()
    {
        CheckSwitchState();

        // update last grounded time
        if (_ctx.CharacterController.isGrounded) _ctx.LastGroundedTime = Time.time;
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (!_ctx.CharacterController.isGrounded && GroundedBuffer())
        {
            SwitchState(_ctx.Factory.Airborne());
        }
    }
    public override void InitializeSubState()
    {
        if (_ctx.DetectPlayer) { SetSubState(_ctx.Factory.Chase()); }

        else SetSubState(_ctx.Factory.Observe());
    }

    private bool GroundedBuffer()
    {
        float _initialJumpTime = Time.time;
        if (_initialJumpTime - _ctx.LastGroundedTime > _ctx.MinimalJumpTime)
        {
            return true;
        }

        return false;
    }
}
