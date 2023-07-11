using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event EventHandler OnJumpPressed;
    public event EventHandler OnJumpReleased;
    public event EventHandler OnAttackPressed;
    public event EventHandler OnAttackReleased;
    public event EventHandler OnSpecialPressed;
    public event EventHandler OnSpecialReleased;

    public event EventHandler OnBackPressed;

    private InputSystem inputSystem;

    private Camera mainCamera;

    private void Awake() {
        mainCamera = FindObjectOfType<Camera>();
        inputSystem = new InputSystem();

        inputSystem.Player.Enable();
        inputSystem.Player.Jump.started+= Jump_performed;
        inputSystem.Player.Jump.canceled += Jump_released;
        inputSystem.Player.Attack.started += Attack_started;
        inputSystem.Player.Attack.canceled += Attack_canceled;
        inputSystem.Player.Special.started += Special_started;
        inputSystem.Player.Special.canceled += Special_canceled;
        inputSystem.Player.Back.started += Back_started;
    }

    private void Special_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSpecialReleased?.Invoke(this, EventArgs.Empty);
    }

    private void Special_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSpecialPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnAttackReleased?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnAttackPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_released(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJumpReleased?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJumpPressed?.Invoke(this, EventArgs.Empty);
    }
    private void Back_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnBackPressed?.Invoke(this, EventArgs.Empty);
    }

    public float GetMoveInput() {
        float moveInput = inputSystem.Player.Movement.ReadValue<float>();
        return moveInput;
    }

    public Vector3 GetMousePositionWorldSpace() {
        Vector2 mousePosition = inputSystem.Player.MouseAim.ReadValue<Vector2>();

        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
        return (mousePosition);
    }
}
