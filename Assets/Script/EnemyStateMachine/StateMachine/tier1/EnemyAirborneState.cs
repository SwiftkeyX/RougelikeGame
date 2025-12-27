using UnityEngine;

public class EnemyAirborneState : EnemyBaseState
{
    public EnemyAirborneState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.FIRST;
        InitializeSubState();
    }

    public override void EnterState()
    {
        
    }
    public override void UpdateState()
    {
        CheckSwitchState();
        HandleGravity();
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (_ctx.CharacterController.isGrounded)
        {
            SwitchState(_ctx.Factory.Grounded());
        }
    }
    public override void InitializeSubState()
    {
        if (_ctx.CurrentMovementY >= 0f) SetSubState(_ctx.Factory.Jump());
        
        else if (_ctx.CurrentMovementY < 0f) SetSubState(_ctx.Factory.Fall());
    }

    private void HandleGravity()
    {
        float gravity = 9.8f;
        _ctx.CurrentMovementY -= gravity;
    }
}
