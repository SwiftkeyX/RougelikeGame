using System.Collections;
using UnityEngine;
using System.Linq;


public class StateMachineHelper
{
    private EnemyStateMachine _ctx;

    public StateMachineHelper(EnemyStateMachine ctx)
    {
        this._ctx = ctx;
    }

    /// <summary>
    /// Use agent tell enemy where is the Player?
    /// </summary>
    public void UpdateAgentToPlayer()
    {
        if (_ctx.IsAgentStop)
        {
            return;
        }

        // agent Pathfinding to player
        _ctx.AgentSetDes(_ctx.PlayerPosition);

        // set that movement to context
        _ctx.CurrentMovement = _ctx.CalculatePath();

        // update agent
        _ctx.AgentUpdateCurrentPosition();
    }

    public void StartAgent() => _ctx.IsAgentStop = false;

    public void StopAgent()
    {
        _ctx.IsAgentStop = true;
        _ctx.Agent.isStopped = true;
        _ctx.Agent.ResetPath();
    }



    /// =============================== combat ===============================
    public bool PlayerInRange()
    {
        float distance = DistanceToPlayer();
        return (distance <= _ctx.AttackRange);
    }

    public float DistanceToPlayer()
    {
        return Vector3.Distance(_ctx.transform.position, _ctx.PlayerPosition);
    }

    public bool DesignWhichAttackToUse()
    {
        float playerDis = DistanceToPlayer();

        // get enemy's attack weight
        Weight weight = new Weight(50f, 50f, 10f, 20f, 15f);

        // get enemy's attack data
        EnemyAttackType light = _ctx.EnemyStat.GetEnemyAttack(ENEMYATTACKTYPE.LIGHT);
        EnemyAttackType heavy = _ctx.EnemyStat.GetEnemyAttack(ENEMYATTACKTYPE.HEAVY);
        EnemyAttackType special = _ctx.EnemyStat.GetEnemyAttack(ENEMYATTACKTYPE.SPECIAL);
        EnemyAttackType gapcloser = _ctx.EnemyStat.GetEnemyAttack(ENEMYATTACKTYPE.GAPCLOSER);
        EnemyAttackType range = _ctx.EnemyStat.GetEnemyAttack(ENEMYATTACKTYPE.RANGE);
        EnemyAttackType[] attackType = new EnemyAttackType[5] { light, heavy, special, gapcloser, range };

        // Change weight base on Distance to the player && attack cooldown 
        // weight -1 = can't be choose by weight
        if (playerDis > light.range || !_ctx.EnemyStat.IsFinishCooldown(ENEMYATTACKTYPE.LIGHT)) { weight.LIGHT = -1f; }
        if (playerDis > heavy.range || !_ctx.EnemyStat.IsFinishCooldown(ENEMYATTACKTYPE.HEAVY)) { weight.HEAVY = -1f; }
        if (playerDis > special.range || !_ctx.EnemyStat.IsFinishCooldown(ENEMYATTACKTYPE.SPECIAL)) { weight.SPECIAL = -1f; }
        if (playerDis > gapcloser.range || !_ctx.EnemyStat.IsFinishCooldown(ENEMYATTACKTYPE.GAPCLOSER)) { weight.GAPCLOSER = -1f; }
        if (playerDis > range.range || !_ctx.EnemyStat.IsFinishCooldown(ENEMYATTACKTYPE.RANGE)) { weight.RANGE = -1f; }

        // Design attack using weight
        ENEMYATTACKTYPE? attackChoose = DesignByWeight(weight);

        // in case, attack isn't choosed
        if (attackChoose == null) return false;

        // set attack on cooldown
        if (attackChoose.HasValue) _ctx.StartCoroutine(CooldownPerAttackCoroutine(attackChoose.Value));

        // set trigger below int to avoid Wrong attack trigger
        _ctx.Animator.SetInteger(_ctx.AttackID, (int)attackChoose);
        _ctx.Animator.SetTrigger(_ctx.IsAttackHash);

        return true;
    }

    /// <summary>
    /// we will random the weight between (0 - Total weight)
    /// and that random weight will trigger different attack 
    /// </summary>
    private ENEMYATTACKTYPE? DesignByWeight(Weight weight)
    {
        float[] weightArray = weight.ToArray();
        // Debug.Log("Weights: " + string.Join(", ", weightArray.Select(w => w.ToString())));

        // get total weight
        float totalWeight = 0f;
        for (int i = 0; i < weightArray.Length; i++)
        {
            totalWeight += weightArray[i];
        }

        // choose attack base on random weight
        float random = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        ENEMYATTACKTYPE attackChoose; // fallback
        for (int i = 0; i < weightArray.Length; i++)
        {
            // -1 = that attack can't be choose
            if (weightArray[i] == -1f) continue;

            currentWeight += weightArray[i];

            if (currentWeight >= random)
            {
                attackChoose = (ENEMYATTACKTYPE)i;
                // Debug.Log("random: " + random + " currentWeight: " + currentWeight + " totalWeight: " + totalWeight);
                return attackChoose;
            }
        }

        return null;
    }

    private IEnumerator CooldownPerAttackCoroutine(ENEMYATTACKTYPE attackChoose)
    {
        _ctx.EnemyStat.StartCooldown(attackChoose);

        while (!_ctx.EnemyStat.IsFinishCooldown(attackChoose))
        {
            _ctx.EnemyStat.TickCooldown(attackChoose);
            yield return null;
        }
    }



    /// =============================== movement ===============================
    public void PreventSlide()
    {
        _ctx.CurrentMovement = new Vector3(0, _ctx.CurrentMovementY, 0);
    }
}