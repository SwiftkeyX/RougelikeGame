

using UnityEngine.AI;

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
        // agent Pathfinding to player
        _ctx.AgentSetDes(_ctx.PlayerPosition);

        // set that movement to context
        _ctx.CurrentMovement = _ctx.CalculatePath();

        // update agent
        _ctx.AgentUpdateCurrentPosition();
    }

    public void StopAgent()
    {
        _ctx.Agent.isStopped = true;
        _ctx.Agent.ResetPath();
    }
}