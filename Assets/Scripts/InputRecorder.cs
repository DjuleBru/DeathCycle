using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRecorder : MonoBehaviour
{
    private InputManager inputManager;
    private ChampionActions championActionsThisFrame = new ChampionActions();

    private bool jumpPressed;
    private bool jumpReleased;
    private bool attackPressed;
    private bool attackReleased;

    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();

        inputManager.OnJumpPressed += InputManager_OnJumpPressed;
        inputManager.OnJumpReleased += InputManager_OnJumpReleased;
        inputManager.OnAttackPressed += InputManager_OnAttackPressed;
        inputManager.OnAttackReleased += InputManager_OnAttackReleased;
    }


    private void Update() {

        championActionsThisFrame.moveDir = inputManager.GetMoveInput();
        championActionsThisFrame.mousePos = inputManager.GetMousePositionWorldSpace();

        if (jumpPressed) {
            championActionsThisFrame.JumpPressed = true;
            jumpPressed = false;
        } else {
            championActionsThisFrame.JumpPressed = false;
        }

        if (jumpReleased) {
            championActionsThisFrame.JumpReleased = true;
            jumpReleased = false;
        } else {
            championActionsThisFrame.JumpReleased = false;
        }

        if (attackPressed) {
            championActionsThisFrame.AttackPressed = true;
            attackPressed = false;
        } else {
            championActionsThisFrame.AttackPressed = false;
        }

        if (attackReleased) {
            championActionsThisFrame.AttackReleased = true;
            attackReleased = false;
        } else {
            championActionsThisFrame.AttackReleased = false;
        }

    }

    private void InputManager_OnAttackReleased(object sender, System.EventArgs e) {
        attackReleased = true;
    }

    private void InputManager_OnAttackPressed(object sender, System.EventArgs e) {
        attackPressed = true;
    }

    private void InputManager_OnJumpReleased(object sender, System.EventArgs e) {
        jumpReleased = true;
    }

    private void InputManager_OnJumpPressed(object sender, System.EventArgs e) {
        jumpPressed = true;
    }

    public ChampionActions GetChampionActionsThisFrame() {
        return championActionsThisFrame;
    }
}
