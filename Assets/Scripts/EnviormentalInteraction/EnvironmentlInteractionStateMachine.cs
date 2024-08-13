using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnvironmentlInteractionStateMachine : StateManager<EnvironmentlInteractionStateMachine.EEnviromentlInteractionState>
{
    [SerializeField] private TwoBoneIKConstraint leftIKConstraint;   
    [SerializeField] private TwoBoneIKConstraint rightIKConstraint;
    [SerializeField] private MultiRotationConstraint leftMultiRotationConstraint;    
    [SerializeField] private MultiRotationConstraint rightMultiRotationConstraint;
    [SerializeField] private CharacterController characterController;
    public enum EEnviromentlInteractionState
    {
        Serach,
        Approach,
        Rise,
        Touch,
        Reset
    }

    
}
