using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyBaseState
{
    protected enum Order { FIRST, SECOND, THIRD };

    protected EnemyStateMachine _ctx;
    protected EnemyStateFactory _factory;
    protected EnemyBaseState _currentSuperState;
    protected EnemyBaseState _currentSubState;
    protected Order _order;

    public EnemyBaseState CurrentSubState { get { return _currentSubState; } }

    public EnemyBaseState(EnemyStateMachine ctx, EnemyStateFactory enemyStateFactory)
    {
        this._ctx = ctx;
        this._factory = enemyStateFactory;
    }

    public void Initial() => OnEnter();

    private void OnEnter()
    {
        EnterState();
        InitializeSubState();
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
    protected void SwitchState(EnemyBaseState newState)
    {
        if (_order != newState._order) { Debug.Log("Cant assign Lower State's tier to Higher one=>" + " current: " + this._order + " new: " + newState._order); return; }

        if (_order == newState._order && _order == Order.FIRST)
        {
            SetRootState(newState);
        }

        else if (_order == newState._order)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    private void SetSuperState(EnemyBaseState newState)
    {
        if (newState._order != this._order - 1) { Debug.Log("Can't set superState"); return; }

        _currentSuperState = newState;
    }

    protected void SetSubState(EnemyBaseState newState)
    {
        // Only allow newState's order to be lower equal to 1
        if (newState._order != this._order + 1) { Debug.Log("Can't set subState"); return; }

        Debug.Log("set sub-state's state from " + this._currentSubState + " to " + newState);

        // first: Exit all sub-state (dont include itself)
        EnemyBaseState state = this;
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

    protected void SetRootState(EnemyBaseState newState)
    {
        if (newState._order != Order.FIRST) return;

        // first: Exit all sub-state include itself
        EnemyBaseState state = this;
        while (state != null)
        {
            state.ExitState();
            state = state.CurrentSubState;
        }

        // second: Enter new-state and its sub-state (other sub-state will be initial in InitializeSubState() after we Enter here)
        newState.OnEnter();

        // tell context that we change root's state
        Debug.Log("set root's state from " + this + " to " + newState);
        _ctx.CurrentState = newState;

    }
}
