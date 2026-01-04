using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;
    protected PlayerBaseState _currentSuperState;
    protected PlayerBaseState _currentSubState;
    protected bool _isRoot;

    // getter and setter
    public PlayerBaseState CurrentSubState { get { return _currentSubState; } }
    public bool IsRoot { get { return _isRoot; } }

    public PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory playerStateFactory)
    {
        this._ctx = ctx;
        this._factory = playerStateFactory;
    }

    private void OnEnter()
    {
        EnterState();
        InitializeSubState();
    }
    private void OnEnterButHandOverSubState(PlayerBaseState newState)
    {
        EnterState();

        // set Root's substate to new-state(attack) 
        _currentSubState = newState;
        _currentSubState.SetSuperState(this);
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
        // switch current state of the Context
        if (_isRoot)
        {
            SetRootState(newState);
        }
        else
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newState)
    {
        _currentSuperState = newState;
    }

    protected void SetSubState(PlayerBaseState newState)
    {
        // Only allow newState's order to be lower equal to 1
        if (newState.IsRoot != false)
        {
            // Debug.Log("Can't set subState"); 
            return;
        }

        // Debug.Log("set sub-state's state from " + this._currentSubState + " to " + newState);

        // first: Exit all sub-state (dont include itself)
        PlayerBaseState state = this;
        while (state.CurrentSubState != null)
        {
            state = state.CurrentSubState;
            state.ExitState();
        }

        // second: Enter new-state and its sub-state (other sub-state will be initial in InitializeSubState() after we Enter here)
        newState.OnEnter();

        // set subState to new state 
        _currentSubState = newState;
        _currentSubState.SetSuperState(this);
    }

    protected void SetRootState(PlayerBaseState newState)
    {
        if (this._currentSubState != null && this._currentSubState.GetType() == typeof(PlayerAttackState)) SetRootAndSendOverSubState(newState);
        else SetRootStateNormally(newState);
    }

    private void SetRootStateNormally(PlayerBaseState newState)
    {
        if (newState.IsRoot != true) return;

        // first: Exit all sub-state include itself
        PlayerBaseState state = this;
        while (state != null)
        {
            state.ExitState();
            state = state.CurrentSubState;
        }

        // second: Enter new-state and its sub-state (other sub-state will be initial in InitializeSubState() after we Enter here)
        newState.OnEnter();

        // tell context that we change root's state
        // Debug.Log("set root's state from " + this + " to " + newState);
        _ctx.CurrentState = newState;
    }

    /// <summary>
    /// if attack didn't finish, it can get canceled by the root state switching from ground => airborne
    /// that mean if substate is attack, new root state shouldn't initialize new state BUT continue updating() the attack state
    /// summary, 
    /// 1. we have to send attack state over from first root to second root
    /// 2. dont exitstate() attack early
    /// </summary>
    private void SetRootAndSendOverSubState(PlayerBaseState newState)
    {
        if (newState.IsRoot != true) return;

        // first: now this is kinda messy, bc for playerStateMachine 
        // player only have 2 tier of stateMachine (Root/notRoot)
        // if in the future it add to be 3 tier, that's going to break this part
        // summary, if it's was still 2 tier FSM, then this part work fine
        // wat happen here? , we only exit the root here
        PlayerBaseState state = this;
        state.ExitState();

        // second: Enter new-state without Initial new state for the Root BUT hand over the attack state to the new Root
        newState.OnEnterButHandOverSubState(this._currentSubState);

        // tell context that we change root's state
        _ctx.CurrentState = newState;
    }
}
