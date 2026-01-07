using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float _t;
    private ENEMYATTACKTYPE? _type;
    private bool _enableHitboxOnce;
    private bool _disableHitboxOnce;

    public EnemyAttackState(EnemyStateMachine ctx, EnemyStateFactory factory) : base(ctx, factory)
    {
        _order = Order.SECOND;
        _enableHitboxOnce = false;
        _disableHitboxOnce = false;
    }

    public override void EnterState()
    {
        // reset flag
        _ctx.Animator.SetBool(_ctx.IsAttackEndHash, false);
        _type = _ctx.Helper.AttackChoose;
    }
    public override void UpdateState()
    {
        _t = _ctx.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        CheckSwitchState();

        EnableHitbox();
    }
    public override void ExitState()
    {
        _ctx.Animator.SetBool(_ctx.IsAttackEndHash, true);
    }
    public override void CheckSwitchState()
    {
        bool attackAnimationIsOver = IsInAttackState() && (_t >= 0.9f) && !_ctx.Animator.IsInTransition(0);

        if (attackAnimationIsOver) SwitchState(_ctx.Factory.Cooldown());
    }
    public override void InitializeSubState() { }

    private void EnableHitbox()
    {
        float hitboxStart = _ctx.EnemyStat.GetEnemyAttack(_type.Value).attackData.hitboxStart;
        float hitboxEnd = _ctx.EnemyStat.GetEnemyAttack(_type.Value).attackData.hitboxEnd;
        // Collider hitbox = _ctx.EnemyStat.GetEnemyAttack(_type.Value).attackData.collider;
        // Hitbox a = hitbox.gameObject.GetComponent<Hitbox>();
        // a.EnableHitbox();
        if ((_t >= hitboxStart && _t <= hitboxEnd) && !_enableHitboxOnce) { _ctx.Weapon.Hitbox.EnableHitbox(); _enableHitboxOnce = true; }

        else if ((_t >= hitboxEnd) && !_disableHitboxOnce) { _ctx.Weapon.Hitbox.DisableHitbox(); _disableHitboxOnce = true; }
    }

    /// <summary>
    /// check if current state is tag with "Attack"
    /// so dont forget to put every attack with tag "Attack"
    /// </summary>
    private bool IsInAttackState(int layerIndex = 0)
    {
        AnimatorStateInfo stateInfo = _ctx.Animator.GetCurrentAnimatorStateInfo(layerIndex);
        return stateInfo.IsTag("Attack");
    }
}
