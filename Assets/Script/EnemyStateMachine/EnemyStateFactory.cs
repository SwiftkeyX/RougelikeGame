public class EnemyStateFactory
{
    private EnemyStateMachine _ctx;

    public EnemyStateFactory(EnemyStateMachine context)
    {
        this._ctx = context;
    }

    // ===================== TIER 1 =====================
    public EnemyBaseState Grounded()
    {
        return new EnemyGroundedState(_ctx, this);
    }
    public EnemyBaseState Airborne()
    {
        return new EnemyAirborneState(_ctx, this);
    }

    // ===================== TIER 2 ======================
    public EnemyBaseState Attack()
    {
        return new EnemyAttackState(_ctx, this);
    }

    public EnemyBaseState Cooldown()
    {
        return new EnemyCooldownState(_ctx, this);
    }

    public EnemyBaseState Observe()
    {
        return new EnemyObserveState(_ctx, this);
    }

    public EnemyBaseState Chase()
    {
        return new EnemyChaseState(_ctx, this);
    }

    public EnemyBaseState Jump()
    {
        return new EnemyJumpState(_ctx, this);
    }

    public EnemyBaseState Fall()
    {
        return new EnemyFallingState(_ctx, this);
    }

    // =================== TIER 3 ====================
    public EnemyBaseState Idle()
    {
        return new EnemyIdleState(_ctx, this);
    }

    public EnemyBaseState ForwardWalk()
    {
        return new EnemyForwardWalkState(_ctx, this);
    }

    public EnemyBaseState Run()
    {
        return new EnemyRunState(_ctx, this);
    }

    public EnemyBaseState Strafe()
    {
        return new EnemyStrafeState(_ctx, this);
    }
}