using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRecorder : MonoBehaviour
{
    private InputManager inputManager;
    private ChampionActions championActionsThisFrame = new ChampionActions();

    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();
    }
    private void Start() {
        inputManager.OnJumpPressed += InputManager_OnJumpPressed;
        inputManager.OnJumpReleased += InputManager_OnJumpReleased;
        inputManager.OnAttackPressed += InputManager_OnAttackPressed;
        inputManager.OnAttackReleased += InputManager_OnAttackReleased;
    }


    private void FixedUpdate() {
        championActionsThisFrame.moveDir = inputManager.GetMoveInput();
    }

    private void InputManager_OnAttackReleased(object sender, System.EventArgs e) {
        championActionsThisFrame.AttackPressed = false;
        championActionsThisFrame.AttackReleased = true;
    }

    private void InputManager_OnAttackPressed(object sender, System.EventArgs e) {
        championActionsThisFrame.AttackPressed = true;
        championActionsThisFrame.AttackReleased = false;
    }

    private void InputManager_OnJumpReleased(object sender, System.EventArgs e) {
        championActionsThisFrame.JumpReleased = true;
        championActionsThisFrame.JumpPressed = false;
    }

    private void InputManager_OnJumpPressed(object sender, System.EventArgs e) {
        championActionsThisFrame.JumpPressed = true;
        championActionsThisFrame.JumpReleased = false;
    }
}
