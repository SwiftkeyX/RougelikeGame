using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    private bool _finishRoll;
    private bool _canCancelRoll;
    private float _t;

    public PlayerRollState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory)
    : base(ctx, playerStateFactory)
    {
        _isRoot = false;
        _canCancelRoll = false;
        _finishRoll = false;
    }

    public override void EnterState()
    {
        _ctx.IsRolling = true;

        // turn off root motion for easier roll management

        // change movement/rotation before rolling
        _ctx.CurrentMovementX = _ctx.CurrentMovementInputX;
        _ctx.CurrentMovementY = _ctx.CurrentMovementInputY;
        Vector3 RollDirection = new Vector3(_ctx.CurrentMovementInputX, 0f, _ctx.CurrentMovementInputY);
        _ctx.RollRotation = Quaternion.LookRotation(RollDirection);
        
        // start roll
        _ctx.Animator.SetTrigger(_ctx.IsRollTriggerHash);
    }
    public override void UpdateState()
    {
        _t = _ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        CancelWindow();

        CheckSwitchState();
    }

    public override void ExitState()
    {
        _ctx.IsRolling = false;

        // turn it back
        _ctx.Animator.applyRootMotion = true;
    }

    public override void CheckSwitchState()
    {
        if (_finishRoll && !_ctx.IsMovementPressed) SwitchState(_factory.Idle());

        else if ((_canCancelRoll || _finishRoll) && _ctx.IsMovementPressed) SwitchState(_factory.Move());

        else if ((_canCancelRoll || _finishRoll) && _ctx.IsAttackPressed) SwitchState(_factory.Attack());
    }

    public override void InitializeSubState() { }

    private void CancelWindow()
    {
        bool IsInRollState = _ctx.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Roll");

        if (IsInRollState && (_t >= 0.6f && _t <= 0.9f)) _canCancelRoll = true;

        if (IsInRollState && (_t >= 0.9f)) _finishRoll = true;
    }
}
