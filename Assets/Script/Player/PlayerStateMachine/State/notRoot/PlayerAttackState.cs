using UnityEditor.MPE;
using UnityEngine;

/// <summary>
/// To make this work properly, we have to include the script "AttackWindpowBuffer.cs" correctly too
/// bc that script work directly dependent to this script
/// </summary>
public class PlayerAttackState : PlayerBaseState
{
    private float _attackWindowStart;
    private float _attackWindowEnd;
    private float _allowBufferWindow;
    private bool _chainAttackImmediately;
    private bool _isAttackBuffer;
    private bool _allowChainAttack;
    private float _t;

    public PlayerAttackState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory) : base(ctx, playerStateFactory)
    {
        _isRoot = false;
        _chainAttackImmediately = false;
        _isAttackBuffer = false;
        _allowChainAttack = false;
    }

    /// <summary>
    /// make isAttackBuffer && AttackID correct to the next attack transition's condition
    /// so animation start 
    /// </summary>
    public override void EnterState()
    {
        // make isAttackTrigger && AttackID correct to the next attack transition's condition
        _ctx.Animator.SetTrigger(_ctx.IsAttackTriggerHash);
        _ctx.Animator.SetBool(_ctx.IsAttackEndHash, false);

        // get window timing for this current AttackID + guard
        if (_ctx.Weapon.Buffer.AttackWindows.Length > _ctx.AttackIDValue)
        {
            _attackWindowStart = _ctx.Weapon.Buffer.AttackWindows[_ctx.AttackIDValue].start;
            _attackWindowEnd = _ctx.Weapon.Buffer.AttackWindows[_ctx.AttackIDValue].end;
        }
        else
        {
            Debug.LogError("AttackID: " + _ctx.AttackIDValue);
        }

        // get allowBufferWindow for current AttackID
        _allowBufferWindow = Mathf.Max(0f, _attackWindowStart - 0.3f);

        Debug.Log("AttackID: " + _ctx.AttackIDValue);
    }

    /// <summary>
    /// player can chain-attack in attack state to transition to next attack animation
    /// If player is in mid first-attack, player can buffer attack. So that when the chain-window arrive, the second-attack is automatically played
    /// we set the "chain-window" first, then 0.3 sec before the "chain-window" arrive is the "buffer-window"
    /// AttackID is used in the transition's condition to next attack animation
    /// </summary>
    public override void UpdateState()
    {
        // t should update every frame
        _t = _ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        IsAllowChainAttack();

        LockMovementWhileAttack();

        CheckSwitchState();
    }
    public override void ExitState()
    {
        bool IfFinalAttack = !(_ctx.Weapon.Buffer.AttackWindows.Length > _ctx.AttackIDValue + 1);

        // if not chain-attack mean transition to other state (ex. idle, move), reset attackID to 0
        // if attack is finish (no combo available), also exit the attack state
        if (!_chainAttackImmediately || IfFinalAttack)
        {
            _ctx.Animator.SetInteger(_ctx.AttackIDHash, 0);
            _ctx.Animator.SetBool(_ctx.IsAttackEndHash, true);   // flag that tell animator when to exit attack animation
            _ctx.Animator.ResetTrigger(_ctx.IsAttackTriggerHash);   // the attackTrigger still true after attack fire, so let reset it manually
        }

        else if (_chainAttackImmediately)
        {
            // increment AttackID, to make it transition to next-attack
            _ctx.Animator.SetInteger(_ctx.AttackIDHash, _ctx.AttackIDValue + 1);
            Debug.Log("chain attack immedi");
        }

        // reset movement lock
        _ctx.MovementLock = false;
    }
    public override void CheckSwitchState()
    {
        bool cancelAnimationToMovement = IsInAttackState() && (_t >= _attackWindowEnd);
        bool attackAnimationIsOver = IsInAttackState() && (_t >= 1f) && !_ctx.Animator.IsInTransition(0);
        bool animatorIsInTransitionToOtherState = !IsInAttackState() && _ctx.Animator.IsInTransition(0);

        // if player want to chain-attack, exit the old-attack first then enter new-attack
        if (_chainAttackImmediately) SwitchState(_factory.Attack());

        else if (cancelAnimationToMovement && _ctx.IsMovementPressed)
        {
            SwitchState(_factory.Move());
            Debug.Log("changing from attack to move => " + " t: " + _t + " isInAttackState: " + IsInAttackState());
        }

        else if ((attackAnimationIsOver) && !_ctx.IsMovementPressed)
        {
            SwitchState(_factory.Idle());
            Debug.Log("changing from attack to Idle");
        }
    }
    public override void InitializeSubState() { }

    private void IsAllowChainAttack()
    {
        bool animatorNotInTransition = !_ctx.Animator.IsInTransition(0);

        // chain-attack when AllowChainAttack and Attack is buffered
        if (_allowChainAttack && _isAttackBuffer)
        {
            _chainAttackImmediately = true;
        }

        // allow buffer-window
        if (animatorNotInTransition && (_t >= _allowBufferWindow && _t <= _attackWindowEnd) && _ctx.IsAttackPressed) { _isAttackBuffer = true; Debug.Log("Attack is buffer"); }

        // allow chain-attack if timing is correct
        if (animatorNotInTransition && (_t >= _attackWindowStart && _t <= _attackWindowEnd)) { _allowChainAttack = true; }
    }

    private void LockMovementWhileAttack()
    {
        // lock the player from moving
        _ctx.MovementLock = true;

    }

    /// <summary>
    /// check if current state is tag with "Attack"
    /// </summary>
    private bool IsInAttackState(int layerIndex = 0)
    {
        AnimatorStateInfo stateInfo = _ctx.Animator.GetCurrentAnimatorStateInfo(layerIndex);
        return stateInfo.IsTag("Attack");
    }
}