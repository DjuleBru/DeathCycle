using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAim : MonoBehaviour
{
    [SerializeField] private Transform weaponEndPointPosition;
    private Transform aimTransform;
    private InputManager inputManager;

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs {
        public Vector3 weaponEndPointPosition;
        public Vector3 shootPosition;
    }

    private void Awake() {
        aimTransform = transform.Find("Aim");
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start() {
        inputManager.OnAttackPressed += InputManager_OnAttackPressed;
    }

    private void InputManager_OnAttackPressed(object sender, EventArgs e) {

        Vector3 mousePosition = inputManager.GetMousePositionWorldSpace();

        OnShoot?.Invoke(this, new OnShootEventArgs {
            weaponEndPointPosition = weaponEndPointPosition.position,
            shootPosition = mousePosition
        });
    }

    private void Update() {
        HandleAim();
    }

    private void HandleAim() {
        Vector3 mousePosition = inputManager.GetMousePositionWorldSpace();

        Vector3 aimDir = (mousePosition - transform.position).normalized;
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
}
