using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAim : MonoBehaviour
{

    [SerializeField] private Transform weaponEndPointPosition;
    private ChampionActions championActionsThisFrame = new ChampionActions();
    private Champion champion;
    private ChampionSO championSO;

    private Transform aimTransform;
    private InputManager inputManager;

    private float championAttackMaxRate;
    private float championAttackTimer;

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs {
        public Vector3 weaponEndPointPosition;
        public Vector3 attackDir;
    }

    private void Awake() {
        champion = GetComponent<Champion>();
        aimTransform = transform.Find("Aim");
        inputManager = FindObjectOfType<InputManager>();
        championSO = champion.ChampionSO;

        inputManager.OnAttackPressed += InputManager_OnAttackPressed;
    }

    private void Start() {
        championAttackMaxRate = championSO.championAttackMaxRate;
    }

    private void Update() {
        championAttackTimer += Time.deltaTime;

        if (LoopManager.Instance.IsRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            Vector3 mousePosition = inputManager.GetMousePositionWorldSpace();
            Vector3 aimDir = (mousePosition - transform.position).normalized;

            // Loop is recording and champion's active loop
            HandleAim(aimDir);
        }

        #region AIM PLAYBACK
        if (!LoopManager.Instance.IsRecording || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) {
            // Loop is not recording OR not champion's active loop

            Vector3 mousePosition = championActionsThisFrame.mousePos;
            Vector3 aimDir = (mousePosition - transform.position).normalized;

            HandleAim(aimDir);
            #endregion

        #region ATTACK PLAYBACK
            if (championActionsThisFrame.AttackPressed == true) {

                if (championAttackTimer >= championAttackMaxRate) {

                    Vector3 attackDir = mousePosition - weaponEndPointPosition.position;

                    
                    OnShoot?.Invoke(this, new OnShootEventArgs {
                        weaponEndPointPosition = weaponEndPointPosition.position,
                        attackDir = attackDir
                    });
                    championAttackTimer = 0f;
                }
            }
        }
        #endregion
    }

    private void InputManager_OnAttackPressed(object sender, EventArgs e) {
        if (LoopManager.Instance.IsRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            if (championAttackTimer >= championAttackMaxRate) {

                Vector3 mousePosition = inputManager.GetMousePositionWorldSpace();
                Vector3 attackDir = mousePosition - weaponEndPointPosition.position;

                OnShoot?.Invoke(this, new OnShootEventArgs {
                    weaponEndPointPosition = weaponEndPointPosition.position,
                    attackDir = attackDir
                });
                championAttackTimer = 0f;
            }
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
