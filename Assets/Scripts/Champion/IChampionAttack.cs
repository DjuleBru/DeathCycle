using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChampionAttack {

    public event EventHandler<OnAttackEventArgs> OnAttack;
    public class OnAttackEventArgs : EventArgs {
        public Vector3 attackDir;
        public string attackType;
    }
    public void SetChampionActionsThisFrame(ChampionActions championActions);
    public void ResetAttacks();

    public bool IsAttacking {
        get;
    }

    public void SetIsAttacking(bool isAttacking) {
    }

    public void IsAttacking1(bool isAttacking1) {
    }
    public void IsAttacking2(bool isAttacking2) {
    }
    public void IsAttacking3(bool isAttacking3) {
    }

    public void DisableAttacks() {
    }

    public void EnableAttacks() {
    }
}