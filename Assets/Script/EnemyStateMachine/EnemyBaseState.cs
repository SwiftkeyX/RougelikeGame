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
        ExitState();

        newState.EnterState();

        /// switch current state of the Context
        // switch same tier's state
        if (_order == newState._order && _order == Order.FIRST)
        {
            _ctx.CurrentState = newState;
        }
        else if (_order == newState._order)
        {
            _currentSuperState.SetSubState(newState);
        }
        else
        {
            Debug.Log("Cant assign Lower State's tier to Higher one=>" + " current: " + this._order + " new: " + newState._order);
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

        /// exit all previous SubState before setting new SubState
        EnemyBaseState state = this;
        while (state.CurrentSubState != null)
        {
            state = state.CurrentSubState;
            state.ExitState();
        }

        // enter new state
        newState.EnterState();

        // set subState to new state 
        _currentSubState = newState;
        _currentSubState.SetSuperState(this);
    }
}
