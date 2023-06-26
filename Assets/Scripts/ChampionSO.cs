using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ChampionSO : ScriptableObject
{
    #region CHAMPION UNIQUENESS
    public GameObject Champion;
    public float moveSpeed;
    #endregion

    #region CHAMPION MOVEMENT PARAMETERS
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

}