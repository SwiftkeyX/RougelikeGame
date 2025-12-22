class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory)
    {
        _isRoot = true; 
    }

    public override void EnterState() { }
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
            SwitchState(_factory.Grounded());
        }
    }
    public override void InitializeSubState() { }

    private void HandleGravity()
    {
        float gravity = 9.8f;
        _ctx.CurrentMovementY -= gravity;

    }
}