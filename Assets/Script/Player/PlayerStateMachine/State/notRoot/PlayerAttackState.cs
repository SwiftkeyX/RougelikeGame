using UnityEditor.MPE;
using UnityEngine;

/// <summary>
/// To make this work properly, we have to include the script "AttackWindpowData.cs" correctly too
/// bc that script work directly dependent to this script
/// </summary>
public class PlayerAttackState : PlayerBaseState
{
    // chain attack
    private float _chainAttackWindowStart;
    private float _chainAttackWindowEnd;
    private float _allowDataWindow;
    private bool _chainAttackImmediately;
    private bool _isAttackData;
    private bool _allowChainAttack;
    // hitbox
    private float _hitboxStart;
    private float _hitboxEnd;
    private bool _enableHitboxOnlyOnce;
    private bool _disableHitboxOnlyOnce;
    // etc
    private float _t;

    public PlayerAttackState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory) : base(ctx, playerStateFactory)
    {
        _isRoot = false;
        _chainAttackImmediately = false;
        _isAttackData = false;
        _allowChainAttack = false;
        _enableHitboxOnlyOnce = false;
        _disableHitboxOnlyOnce = false;
    }

    /// <summary>
    /// make isAttackData && AttackID correct to the next attack transition's condition
    /// so animation start 
    /// </summary>
    public override void EnterState()
    {
        // make isAttackTrigger && AttackID correct to the next attack transition's condition
        _ctx.Animator.SetTrigger(_ctx.IsAttackTriggerHash);
        _ctx.Animator.SetBool(_ctx.IsAttackEndHash, false);

        // get window timing for this current AttackID + guard
        if (_ctx.Weapon.Data.AttackDatas.Length > _ctx.AttackIDValue)
        {
            _chainAttackWindowStart = _ctx.Weapon.Data.AttackDatas[_ctx.AttackIDValue].chainStart;
            _chainAttackWindowEnd = _ctx.Weapon.Data.AttackDatas[_ctx.AttackIDValue].chainEnd;
            _hitboxStart = _ctx.Weapon.Data.AttackDatas[_ctx.AttackIDValue].hitboxStart;
            _hitboxEnd = _ctx.Weapon.Data.AttackDatas[_ctx.AttackIDValue].hitboxEnd;
        }
        else
        {
            Debug.LogError("AttackID: " + _ctx.AttackIDValue);
        }

        // get allowDataWindow for current AttackID
        _allowDataWindow = Mathf.Max(0f, _chainAttackWindowStart - 0.3f);

        LockMovementWhileAttack(true);
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

        _ctx.BufferInput.CanRollBuffer(this);

        EnableHitbox();

        IsAllowChainAttack();

        CheckSwitchState();
    }

    public override void ExitState()
    {
        bool IfFinalAttack = !(_ctx.Weapon.Data.AttackDatas.Length > _ctx.AttackIDValue + 1);

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
            // Debug.Log("chain attack immedi");
        }

        // reset movement lock
        LockMovementWhileAttack(false);

        // reset roll buffer
        _ctx.BufferInput.IsRollBuffer = false;
    }

    public override void CheckSwitchState()
    {
        bool cancelAnimationToRoll = IsInAttackState() && (_t >= _chainAttackWindowEnd - 0.2f);
        bool cancelAnimationToMovement = IsInAttackState() && (_t >= _chainAttackWindowEnd);
        bool attackAnimationIsOver = IsInAttackState() && (_t >= 1f) && !_ctx.Animator.IsInTransition(0);

        // if player want to chain-attack, exit the old-attack first then enter new-attack
        if (_chainAttackImmediately) SwitchState(_factory.Attack());

        else if (cancelAnimationToMovement && _ctx.BufferInput.IsRollBuffer)
        {
            SwitchState(_factory.Roll());
        }

        else if (cancelAnimationToMovement && _ctx.IsMovementPressed)
        {
            SwitchState(_factory.Move());
            // Debug.Log("changing from attack to move => " + " t: " + _t + " isInAttackState: " + IsInAttackState());
        }

        else if ((attackAnimationIsOver) && !_ctx.IsMovementPressed)
        {
            SwitchState(_factory.Idle());
            // Debug.Log("changing from attack to Idle");
        }
    }

    public override void InitializeSubState() { }

    private void IsAllowChainAttack()
    {
        bool animatorNotInTransition = !_ctx.Animator.IsInTransition(0);

        // chain-attack when AllowChainAttack and Attack is buffered
        if (_allowChainAttack && _isAttackData)
        {
            _chainAttackImmediately = true;
        }

        // allow buffer-window
        if (animatorNotInTransition && (_t >= _allowDataWindow && _t <= _chainAttackWindowEnd) && _ctx.IsAttackPressed) { _isAttackData = true; }

        // allow chain-attack if timing is correct
        if (animatorNotInTransition && (_t >= _chainAttackWindowStart && _t <= _chainAttackWindowEnd)) { _allowChainAttack = true; }
    }

    private void EnableHitbox()
    {
        if ((_t >= _hitboxStart && _t < _hitboxEnd) && !_enableHitboxOnlyOnce) { _ctx.Weapon.Hitbox.EnableHitbox(); _enableHitboxOnlyOnce = true; }

        else if (_t >= _hitboxEnd && !_disableHitboxOnlyOnce) { _ctx.Weapon.Hitbox.DisableHitbox(); _disableHitboxOnlyOnce = true; }
    }

    private void LockMovementWhileAttack(bool value)
    {
        // lock the player from moving
        _ctx.MovementLock = value;

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