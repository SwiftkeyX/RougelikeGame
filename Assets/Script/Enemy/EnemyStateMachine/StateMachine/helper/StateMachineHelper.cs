using UnityEngine;

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

    /// combat
    public bool PlayerInRange()
    {
        float distance = Vector3.Distance(_ctx.transform.position, _ctx.PlayerPosition);
        return (distance <= _ctx.AttackRange);
    }

    /// movement
    public void PreventSlide()
    {
        _ctx.CurrentMovement = new Vector3(0, _ctx.CurrentMovementY, 0);
    }
}