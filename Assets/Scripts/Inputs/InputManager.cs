using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerMovement playerInput;
    //private PlayerInput inputs;
    private PlayerMovement.OnFootActions onFootAction;
    private PlayerMovement.TouchActions touchActions;
    //private InputAction touchPositionAction;
    //private InputAction touchPressAction;
    private PlayerMotor motor;
    private PlayerLook look;

    private void OnEnable()
    {
        onFootAction.Enable();
       // touchActions.TouchPress.performed += TouchPressed;
        //touchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        onFootAction.Disable();
       // touchActions.TouchPress.performed -= TouchPressed;
        //touchPressAction.performed -= TouchPressed;
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInput = new PlayerMovement();
        onFootAction = playerInput.OnFoot;
        touchActions = playerInput.Touch;
        //touchPressAction = inputs.actions.FindAction("TouchPress");
        //touchPositionAction = inputs.actions["TouchPosition"];
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        AssignInputs();
    }

    private void FixedUpdate()
    {
        //motor.ProcessMovement(touchActions.TouchPosition.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFootAction.Look.ReadValue<Vector2>());
    }

    private void AssignInputs() 
    {
        onFootAction.Jump.performed += ctx => motor.Jump();
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        Debug.Log(value);
    }

}
