using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event EventHandler OnJumpPressed;
    public event EventHandler OnJumpReleased;
    private InputSystem inputSystem;

    private void Awake() {
        inputSystem = new InputSystem();

        inputSystem.Player.Enable();

        inputSystem.Player.Jump.performed += Jump_performed;
        inputSystem.Player.Jump.canceled += Jump_released;
    }

    private void Jump_released(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJumpReleased?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJumpPressed?.Invoke(this, EventArgs.Empty);
    }

    public float GetMoveInput() {
        float moveInput = inputSystem.Player.Movement.ReadValue<float>();
        return moveInput;
    }
}
