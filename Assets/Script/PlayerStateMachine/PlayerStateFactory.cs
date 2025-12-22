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

    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_ctx, this);
    }

    public PlayerBaseState Run()
    {
        return new PlayerRunState(_ctx, this);
    }

    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_ctx, this);
    }

    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_ctx, this);
    }
}