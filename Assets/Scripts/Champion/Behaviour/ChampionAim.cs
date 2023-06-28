using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAim : MonoBehaviour
{

    [SerializeField] private Transform weaponEndPointPosition;
    private ChampionActions championActionsThisFrame = new ChampionActions();
    private Champion champion;

    private Transform aimTransform;
    private InputManager inputManager;
    private InputRecorder inputRecorder;

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs {
        public Vector3 weaponEndPointPosition;
        public Vector3 attackDir;
    }

    private void Awake() {
        champion = GetComponent<Champion>();
        aimTransform = transform.Find("Aim");
        inputManager = FindObjectOfType<InputManager>();
        inputRecorder = FindObjectOfType<InputRecorder>();
    }

    private void Start() {
        inputManager.OnAttackPressed += InputManager_OnAttackPressed;
    }

    private void Update() {
        if (LoopManager.Instance.IsRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            Vector3 mousePosition = inputManager.GetMousePositionWorldSpace();
            Vector3 aimDir = (mousePosition - transform.position).normalized;

            // Loop is recording and champion's active loop
            HandleAim(aimDir);
        }
    }

    private void FixedUpdate() {
        #region AIM PLAYBACK
        if (!LoopManager.Instance.IsRecording || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) {
            // Loop is not recording OR not champion's active loop

            Vector3 mousePosition = championActionsThisFrame.mousePos;
            Vector3 aimDir = (mousePosition - transform.position).normalized;

            HandleAim(aimDir);
        #endregion

        #region ATTACK PLAYBACK
            if (championActionsThisFrame.AttackPressed == true) {
                Vector3 attackDir = mousePosition - weaponEndPointPosition.position;

                OnShoot?.Invoke(this, new OnShootEventArgs {
                    weaponEndPointPosition = weaponEndPointPosition.position,
                    attackDir = attackDir
                });
            }
        }
         #endregion
        }

    private void InputManager_OnAttackPressed(object sender, EventArgs e) {
        if (LoopManager.Instance.IsRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            Vector3 mousePosition = inputManager.GetMousePositionWorldSpace();
            Vector3 attackDir = mousePosition - weaponEndPointPosition.position;

            OnShoot?.Invoke(this, new OnShootEventArgs {
                weaponEndPointPosition = weaponEndPointPosition.position,
                attackDir = attackDir
            });
        }
    }

    private void HandleAim(Vector3 aimDir) {

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90) {
            aimLocalScale.y = -1f;
        } else {
            aimLocalScale.y = +1f;
        }
        aimTransform.localScale = aimLocalScale;
    }

    public void SetChampionActionsThisFrame(ChampionActions championActions) {
        this.championActionsThisFrame = championActions;
    }
}
