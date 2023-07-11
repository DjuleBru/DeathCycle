using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ChampionSO : ScriptableObject
{
    #region CHAMPION UNIQUENESS
    public Champion Champion;
    public Sprite sprite;
    #endregion

    #region CHAMPION MOVEMENT PARAMETERS
    public float moveSpeed;
    public float velPower;
    public float acceleration;
    public float deceleration;
    public float frictionAmount;
    public float jumpForce;
    public float jumpCoyoteTime;
    public float jumpInputBufferTime;
    public float jumpCutMultiplier;
    public float gravityScale;
    public float jumpCutGravityMult;
    public float maxFallSpeed;
    #endregion

    #region CHAMPION HEALTH:DAMAGE PARAMETERS
    public float championMaxHealth;
    public float championAirAttackDamage;
    public float championAttack1Damage;
    public float championAttack2Damage;
    public float championAttack3Damage;
    public float championAttackMaxRate;
    public float championSpecialDamage;
    #endregion

    #region OTHER PARAMETERS
    public int specialManaCost;
    #endregion

}
