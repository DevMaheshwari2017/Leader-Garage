using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerMovement playerInput;
    private PlayerMovement.OnFootActions onFootAction;
    private PlayerMotor motor;
    private PlayerLook look;

    private void OnEnable()
    {
        onFootAction.Enable();
    }

    private void OnDisable()
    {
        onFootAction.Disable();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerInput = new PlayerMovement();
        onFootAction = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        AssignInputs();
    }

    private void FixedUpdate()
    {
        motor.ProcessMovement(onFootAction.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFootAction.Look.ReadValue<Vector2>());
    }

    private void AssignInputs() 
    {
        onFootAction.Jump.performed += ctx => motor.Jump();
    }
}
