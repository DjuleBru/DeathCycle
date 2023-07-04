using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAttack : MonoBehaviour
{
    private ChampionActions championActionsThisFrame = new ChampionActions();

    private ChampionMovement championMovement;
    private Champion champion;
    private ChampionSO championSO;
    private Rigidbody2D rb;

    private InputManager inputManager;

    private float championAttackMaxRate;
    private float championAttackTimer;

    public event EventHandler<OnAttackEventArgs> OnAttack;
    public class OnAttackEventArgs : EventArgs {
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
        championMovement = GetComponent<ChampionMovement>();
        rb = GetComponent<Rigidbody2D>();

        inputManager.OnAttackPressed += InputManager_OnAttackPressed;
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
    }

    private void Start() {
        championAttackMaxRate = championSO.championAttackMaxRate;
    }

    private void Update() {

        championAttackTimer += Time.deltaTime;

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
            // Champion is jumping
            if (championAttackTimer >= championAttackMaxRate) {

                OnAttack?.Invoke(this, new OnAttackEventArgs {
                    attackDir = attackDir,
                    attackCount = 0
                });
            }
            championAttackTimer = 0f;
        }

        if (isAttacking2) {

            OnAttack?.Invoke(this, new OnAttackEventArgs {
                attackDir = attackDir,
                attackCount = 3
            });
            championAttackTimer = 0f;
        }

        if (isAttacking1) {

            OnAttack?.Invoke(this, new OnAttackEventArgs {
                attackDir = attackDir,
                attackCount = 2
            });
            championAttackTimer = 0f;
        }

        if (championAttackTimer >= championAttackMaxRate) {

            OnAttack?.Invoke(this, new OnAttackEventArgs {
                attackDir = attackDir,
                attackCount = 1
            });
            championAttackTimer = 0f;
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
        championAttackTimer = 0f;
    }

    public void IsAttacking1(bool isAttacking1) {
        this.isAttacking1 = isAttacking1;
    }
    public void IsAttacking2(bool isAttacking3) {
        this.isAttacking2 = isAttacking3;
    }
    public void IsAttacking3(bool isAttacking2) {
        this.isAttacking3 = isAttacking2;
    }
}
