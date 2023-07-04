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
    private bool specialPressed;
    private bool specialReleased;

    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();

        inputManager.OnJumpPressed += InputManager_OnJumpPressed;
        inputManager.OnJumpReleased += InputManager_OnJumpReleased;
        inputManager.OnAttackPressed += InputManager_OnAttackPressed;
        inputManager.OnAttackReleased += InputManager_OnAttackReleased;
        inputManager.OnSpecialPressed += InputManager_OnSpecialPressed;
        inputManager.OnSpecialReleased += InputManager_OnSpecialReleased;
    }

    private void Update() {

        championActionsThisFrame.moveDir = inputManager.GetMoveInput();
        championActionsThisFrame.mousePos = inputManager.GetMousePositionWorldSpace();

        championActionsThisFrame.JumpPressed = false;
        championActionsThisFrame.JumpReleased = false;
        championActionsThisFrame.AttackPressed = false;
        championActionsThisFrame.AttackReleased = false;
        championActionsThisFrame.SpecialPressed = false;
        championActionsThisFrame.SpecialReleased = false;


        if (jumpPressed) {
            championActionsThisFrame.JumpPressed = true;
        }

        if (jumpReleased) {
            championActionsThisFrame.JumpReleased = true;
        }

        if (attackPressed) {
            championActionsThisFrame.AttackPressed = true;
        }

        if (attackReleased) {
            championActionsThisFrame.AttackReleased = true;
        }

        if (specialPressed) {
            championActionsThisFrame.SpecialPressed = true;
        }

        if (specialReleased) {
            championActionsThisFrame.SpecialReleased = true;
        }

    }


    private void InputManager_OnSpecialReleased(object sender, System.EventArgs e) {
        specialReleased = true;
    }

    private void InputManager_OnSpecialPressed(object sender, System.EventArgs e) {
        specialPressed = true;
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
