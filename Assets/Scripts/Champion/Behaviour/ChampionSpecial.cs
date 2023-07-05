using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionSpecial : MonoBehaviour, IChampionSpecial
{
    private ChampionActions championActionsThisFrame = new ChampionActions();
    private InputManager inputManager;
    private Champion champion;
    private Rigidbody2D rb;

    public event EventHandler<IChampionSpecial.OnSpecialEventArgs> OnSpecial;

    private bool isSpecialing;

    private bool loopOnPause;
    private bool loopOnRecording;
    private bool loopOnPlaybacking;

    private void Awake() {
        champion = GetComponent<Champion>();
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody2D>();

        inputManager.OnSpecialPressed += InputManager_OnSpecialPressed; ;
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
    }

    private void Update() {
        if (loopOnPlaybacking || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) {
            // Loop is not recording OR not champion's active loop

            Vector3 mousePosition = championActionsThisFrame.mousePos;
            Vector3 specialDir = mousePosition - transform.position;

            if (championActionsThisFrame.SpecialPressed == true) {
                HandleSpecial(specialDir);
            }
        }
    }

    private void HandleSpecial(Vector3 specialDir) {

        if (rb.velocity.y == 0 && !isSpecialing) {
            OnSpecial?.Invoke(this, new IChampionSpecial.OnSpecialEventArgs {
                specialDir = specialDir
            });
        }
    }

    private void InputManager_OnSpecialPressed(object sender, EventArgs e) {
        if (loopOnRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {

            Vector3 mousePosition = inputManager.GetMousePositionWorldSpace();
            Vector3 specialDir = mousePosition - transform.position;

            HandleSpecial(specialDir);
        }
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Pause) {
            loopOnPause = true;
            loopOnRecording = false;
            loopOnPlaybacking = false;
        }
        if (e.state == LoopManager.State.Recording) {
            loopOnPause = false;
            loopOnRecording = true;
            loopOnPlaybacking = false;
        }
        if (e.state == LoopManager.State.Playbacking) {
            loopOnPause = false;
            loopOnRecording = false;
            loopOnPlaybacking = true;
        }
    }

    public void SetChampionActionsThisFrame(ChampionActions championActions) {
        this.championActionsThisFrame = championActions;
    }
    public void ResetSpecial() {
        isSpecialing = false;
    }

    public void IsSpecialing(bool isSpecialing) {
        this.isSpecialing = isSpecialing;
    }

    public void DisableSpecial() {
        this.enabled = false;
    }

    public void EnableSpecial() {
        this.enabled = true;
    }
}
