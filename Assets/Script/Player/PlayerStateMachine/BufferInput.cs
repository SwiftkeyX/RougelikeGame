public class BufferInput
{
    public BufferInput(PlayerStateMachine ctx)
    {
        this._ctx = ctx;
    }

    // dependency
    private PlayerStateMachine _ctx;

    // buffer var
    private bool _isRollBuffer;
    private float _t;
    private float _startBuffer;

    // getter and setter
    public bool IsRollBuffer { get { return _isRollBuffer; } set { _isRollBuffer = value; } }

    public void CanRollBuffer(PlayerBaseState state)
    {
        bool checkIsAnimatorCorrectState;
        bool rollBufferWindow;

        // get what time roll can be buffer base on wat state player is in
        if (state.GetType() == typeof(PlayerIdleState))
        {
            _startBuffer = 0f;
            checkIsAnimatorCorrectState = _ctx.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle") || _ctx.Animator.GetNextAnimatorStateInfo(0).IsTag("Idle");
        }
        else if (state.GetType() == typeof(PlayerMoveState))
        {
            _startBuffer = 0f;
            checkIsAnimatorCorrectState = _ctx.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Move") || _ctx.Animator.GetNextAnimatorStateInfo(0).IsTag("Move");
        }
        else if (state.GetType() == typeof(PlayerAttackState))
        {
            _startBuffer = 0.6f;
            checkIsAnimatorCorrectState = _ctx.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || _ctx.Animator.GetNextAnimatorStateInfo(0).IsTag("Attack");
        }
        else
        {
            _startBuffer = 999f;
            checkIsAnimatorCorrectState = false;
        }

        _t = _ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        rollBufferWindow = (_t > _startBuffer) && checkIsAnimatorCorrectState;

        // buffer roll when roll is pressed and roll's buffer window is on
        if (_ctx.IsRollPressed && rollBufferWindow) _isRollBuffer = true;
    }
}