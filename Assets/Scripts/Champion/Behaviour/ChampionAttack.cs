using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAttack : MonoBehaviour, IChampionAttack
{
    private ChampionActions championActionsThisFrame = new ChampionActions();
    private ChampionAttackType championAttackType = new ChampionAttackType();

    private Champion champion;
    private Rigidbody2D rb;

    private InputManager inputManager;

    public event EventHandler<IChampionAttack.OnAttackEventArgs> OnAttack;

    private bool isAttacking;
    public bool IsAttacking { get { return isAttacking; } }

    private bool isAttacking1;
    private bool isAttacking2;
    private bool isAttacking3;

    private bool loopOnPause;
    private bool loopOnRecording;
    private bool loopOnPlaybacking;

    private void Awake() {
        champion = GetComponent<Champion>();
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody2D>();

        inputManager.OnAttackPressed += InputManager_OnAttackPressed;
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
    }

    private void Update() {
        #region ATTACK PLAYBACK
        if (loopOnPlaybacking || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) {
            // Loop is not recording OR not champion's active loop

            Vector3 mousePosition = championActionsThisFrame.mousePos;
            Vector3 attackDir = mousePosition - transform.position;

            if (championActionsThisFrame.AttackPressed == true) {
                HandleAttacks(attackDir);
            }
        }
        #endregion
    }

    private void InputManager_OnAttackPressed(object sender, EventArgs e) {
        if (loopOnRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {

            Vector3 mousePosition = inputManager.GetMousePositionWorldSpace();
            Vector3 attackDir = mousePosition - transform.position;

            HandleAttacks(attackDir);
        }

    }

    private void HandleAttacks(Vector3 attackDir) {

        if (rb.velocity.y != 0) {
            // Champion is not grounded

            OnAttack?.Invoke(this, new IChampionAttack.OnAttackEventArgs {
                attackDir = attackDir,
                attackType = championAttackType.AIRATTACK
            });
        }

        if (isAttacking2) {

            OnAttack?.Invoke(this, new IChampionAttack.OnAttackEventArgs {
                attackDir = attackDir,
                attackType = championAttackType.ATTACK3
            });
        }

        if (isAttacking1) {

            OnAttack?.Invoke(this, new IChampionAttack.OnAttackEventArgs {
                attackDir = attackDir,
                attackType = championAttackType.ATTACK2
            });
        }

        if (rb.velocity.y == 0 && !isAttacking2 && !isAttacking1) {

            OnAttack?.Invoke(this, new IChampionAttack.OnAttackEventArgs {
                attackDir = attackDir,
                attackType = championAttackType.ATTACK1
            });
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

    public void ResetAttacks() {
        isAttacking1 = false;
        isAttacking2 = false;
        isAttacking3 = false;
    }

    public void SetIsAttacking(bool isAttacking) {
        this.isAttacking = isAttacking;
    }

    public void IsAttacking1(bool isAttacking1) {
        this.isAttacking1 = isAttacking1;
    }
    public void IsAttacking2(bool isAttacking2) {
        this.isAttacking2 = isAttacking2;
    }
    public void IsAttacking3(bool isAttacking3) {
        this.isAttacking3 = isAttacking3;
    }

    public  void DisableAttacks() {
        this.enabled = false;
    }

    public void EnableAttacks() {
        this.enabled = true;
    }
}
