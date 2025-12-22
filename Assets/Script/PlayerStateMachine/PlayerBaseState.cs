using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;
    protected PlayerBaseState _currentSuperState;
    protected PlayerBaseState _currentSubState;
    protected bool _isRoot;

    public PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory)
    {
        this._ctx = ctx;
        this._factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null) _currentSubState.UpdateStates();
    }
    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();
        newState.EnterState();

        // switch current state of the Context
        if (_isRoot)
        {
            _ctx.CurrentState = newState;
        }
        else
        {
            _ctx.CurrentState.SetSubState(newState);
        }
    }
    protected void SetSuperState(PlayerBaseState newState)
    {
        _currentSuperState = newState;
    }
    protected void SetSubState(PlayerBaseState newState)
    {
        _currentSubState = newState;
        _currentSubState.SetSuperState(this);
    }
}
