using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAimMelee : MonoBehaviour
{

    private ChampionActions championActionsThisFrame = new ChampionActions();
    private Champion champion;
    private ChampionSO championSO;

    private InputManager inputManager;

    private float championAttackMaxRate;
    private float championAttackComboBuffer = 0.5f;
    private float championAttackComboTime;
    private float championAttackComboTimer;
    private float championAttackTimer;

    public event EventHandler<OnMeleeAttackEventArgs> OnMeleeAttack;
    public class OnMeleeAttackEventArgs : EventArgs {
        public Vector3 attackDir;
        public int attackCount;
    }

    private bool isAttacking1;
    private bool isAttacking2;
    private bool isAttacking3;

    private bool loopOnPause;
    private bool loopOnRecording;
    private bool loopOnPlaybacking;

    private void Awake() {
        champion = GetComponent<Champion>();
        inputManager = FindObjectOfType<InputManager>();
        championSO = champion.ChampionSO;

        inputManager.OnAttackPressed += InputManager_OnAttackPressed;
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
    }

    private void Start() {
        championAttackMaxRate = championSO.championAttackMaxRate;
        championAttackComboTime = championAttackMaxRate + championAttackComboBuffer;
    }

    private void Update() {
        championAttackTimer += Time.deltaTime;
        championAttackComboTimer += Time.deltaTime;

        if (championAttackComboTimer >= championAttackComboTime && isAttacking1) {
            isAttacking1 = false;
        }
        if (championAttackComboTimer >= championAttackComboTime && isAttacking2) {
            isAttacking2 = false;
        }
        if (championAttackComboTimer >= championAttackComboTime && isAttacking3) {
            isAttacking3 = false;
        }

        #region ATTACK PLAYBACK
        if (loopOnPlaybacking || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) {
            // Loop is not recording OR not champion's active loop

            Vector3 mousePosition = championActionsThisFrame.mousePos;
            Vector3 attackDir = mousePosition - transform.position;

            if (championActionsThisFrame.AttackPressed == true) {
                HandleComboAttacks(attackDir);
            }
        }
        #endregion
    }

    private void InputManager_OnAttackPressed(object sender, EventArgs e) {
        if (loopOnRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {

            Vector3 mousePosition = inputManager.GetMousePositionWorldSpace();
            Vector3 attackDir = mousePosition - transform.position;

            HandleComboAttacks(attackDir);
        }

    }

    private void HandleComboAttacks(Vector3 attackDir) {

        if (isAttacking2 && championAttackComboTimer < championAttackComboTime) {

            isAttacking3 = true;
            OnMeleeAttack?.Invoke(this, new OnMeleeAttackEventArgs {
                attackDir = attackDir,
                attackCount = 3
            });
            championAttackTimer = 0f;
            championAttackComboTimer = 0f;
        }

        if (isAttacking1 && championAttackComboTimer < championAttackComboTime) {

            isAttacking2 = true;
            OnMeleeAttack?.Invoke(this, new OnMeleeAttackEventArgs {
                attackDir = attackDir,
                attackCount = 2
            });
            championAttackTimer = 0f;
            championAttackComboTimer = 0f;
        }

        if (championAttackTimer >= championAttackMaxRate) {

            isAttacking1 = true;
            OnMeleeAttack?.Invoke(this, new OnMeleeAttackEventArgs {
                attackDir = attackDir,
                attackCount = 1
            });
            championAttackTimer = 0f;
            championAttackComboTimer = 0f;
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
}
