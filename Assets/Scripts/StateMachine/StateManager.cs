using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<Estate> : MonoBehaviour where Estate : Enum
{
    private bool isInTransitionState;
    protected Dictionary<Estate, BaseState<Estate>> state = new Dictionary<Estate, BaseState<Estate>>();
    protected BaseState<Estate> currentState;

    void Start()
    {
        currentState.EnterState();
    }
    void Update()
    {
        Estate nextStateKey = currentState.GetNextState();

        if (!isInTransitionState && nextStateKey.Equals(currentState.stateKey))
            currentState.UpdateState();
        else if (!isInTransitionState) 
        {
            TransitionToState(nextStateKey);
        }
    }

    private void TransitionToState(Estate stateKey) 
    {
        isInTransitionState = true;
        currentState.ExitState();
        currentState = state[stateKey];
        currentState.EnterState();
        isInTransitionState = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }

    private void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }
}
