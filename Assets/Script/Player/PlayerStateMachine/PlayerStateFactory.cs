public class PlayerStateFactory
{
    private PlayerStateMachine _ctx;
    
    public PlayerStateFactory(PlayerStateMachine context)
    {
        this._ctx = context;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_ctx, this);
    }

    public PlayerBaseState Move()
    {
        return new PlayerMoveState(_ctx, this);
    }

    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_ctx, this);
    }

    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_ctx, this);
    }

    public PlayerBaseState Roll()
    {
        return new PlayerRollState(_ctx, this);
    }

    public PlayerBaseState Attack()
    {
        return new PlayerAttackState(_ctx, this);
    }
}