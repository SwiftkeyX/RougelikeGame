using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyGroundedState : EnemyBaseState
{
    public EnemyGroundedState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.FIRST;
    }

    public override void EnterState()
    {
        float groundedValue = 0.5f; // apparently the characterController want a little gravity even when we already are on the ground
        _ctx.CurrentMovementY -= groundedValue;
    }
    public override void UpdateState()
    {
        CheckSwitchState();

        // update last grounded time
        if (IsGroundedSphereCast()) { _ctx.LastGroundedTime = Time.time; }
    }
    public override void ExitState() { }
    public override void CheckSwitchState()
    {
        if (GroundedBuffer())
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
            Debug.Log("Pass GroundedBuffer => InitialJumpTime: " + _initialJumpTime + " LastGroundedTime: " + _ctx.LastGroundedTime + " MinimalJumpTime: " + _ctx.MinimalJumpTime);
            return true;
        }

        return false;
    }

    /// <summary>
    /// wat is CheckSphere do here?
    /// Create a sphere around the feet, 
    /// if the sphere have overlap with the ground layer, then consider character is grounded
    /// </summary>
    private bool IsGroundedSphereCast()
    {
        // update sphere origin to enemy's feet every frame
        _ctx.SphereOrigin = _ctx.transform.position + Vector3.up * _ctx.GroundCheckStartOffset;

        // call CheckSphere
        bool isGrounded = Physics.CheckSphere(
            _ctx.SphereOrigin,
            _ctx.GroundedSphereRadius,
            _ctx.GroundLayer,
            QueryTriggerInteraction.Ignore
        );

        return isGrounded;
    }

}
