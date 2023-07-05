using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChampionSpecial {

    public event EventHandler<OnSpecialEventArgs> OnSpecial;
    public class OnSpecialEventArgs : EventArgs {
        public Vector3 specialDir;
    }
    public void SetChampionActionsThisFrame(ChampionActions championActions) {

    }
    public void ResetSpecial() {

    }

    public void IsSpecialing(bool isSpecialing) {
    }

    public void DisableSpecial() {
    }

    public void EnableSpecial() {
    }
}