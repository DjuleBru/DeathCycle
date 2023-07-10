using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChampionActions
{
    public float actionTimeInLoop;
    public float moveDir;
    public Vector3 mousePos;
    public bool JumpPressed;
    public bool JumpReleased;
    public bool AttackPressed;
    public bool AttackReleased;
    public bool SpecialPressed;
    public bool SpecialReleased;

    public ChampionActions() { 

    }
}
