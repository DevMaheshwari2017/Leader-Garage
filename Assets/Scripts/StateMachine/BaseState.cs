using System;
using UnityEngine;

public abstract class BaseState<Estate> where Estate : Enum
{
    public BaseState(Estate key) 
    {
        stateKey = key;
    }

    public Estate stateKey { get; private set; }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract Estate GetNextState();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}
