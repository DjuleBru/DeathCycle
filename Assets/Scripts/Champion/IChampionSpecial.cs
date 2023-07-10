using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChampionSpecial {
    public bool IsSpecialing { get; }

    public event EventHandler<OnSpecialEventArgs> OnSpecial;
    public event EventHandler OnSpecialLackingMana;
    public class OnSpecialEventArgs : EventArgs {
        public Vector3 specialDir;
        public string attackType;
    }
    public void SetChampionActionsThisFrame(ChampionActions championActions) {

    }
    public void ResetSpecial() {

    }

    public void SetIsSpecialing(bool isSpecialing) {
    }

    public void SetIsSpecialingLackingMana(bool isSpecialingLackingMana) { 
    }

    public void DisableSpecial() {
    }

    public void EnableSpecial() {
    }
}