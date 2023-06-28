using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ChampionMovement : MonoBehaviour
{
    private ChampionActions championActionsThisFrame = new ChampionActions();

    private InputManager inputManager;
    private float moveInput;

    private ChampionSO championSO;
    private Champion champion;
    private Rigidbody2D rb;

    #region CHECK PARAMETERS
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

    [SerializeField] private LayerMask _groundLayer;
    #endregion

    #region TIMERS
    private float lastGroundedTime;
    private float lastPressedJumpTime;
    #endregion

    #region BOOL CHECKERS
    private bool isGrounded;
    private bool isJumping;
    private bool isJumpFalling;
    private bool jumpInputReleased;
    private bool isJumpCut;
    #endregion

    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody2D>();
        champion = GetComponent<Champion>();
        championSO = champion.ChampionSO;
    }

    void Start()
    {
        inputManager.OnJumpPressed += InputManager_OnJumpPressed;
        inputManager.OnJumpReleased += InputManager_OnJumpReleased;
    }

    void Update()
    {
        #region TIMERS
        lastGroundedTime -= Time.deltaTime;
        lastPressedJumpTime -= Time.deltaTime;
        #endregion

        #region INPUTHANDLER
        if (LoopManager.Instance.IsRecording) {
            moveInput = inputManager.GetMoveInput();
        }
        #endregion

        #region COLLISION CHECKS
        if (!isJumping) {
            // Grounded check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) {
                lastGroundedTime = championSO.jumpCoyoteTime;
            }
        }
        #endregion

        #region JUMP CHECKS
        if (isJumping && rb.velocity.y < 0f) {
            isJumping = false;
            isJumpFalling = true;
        }

        if (lastGroundedTime > 0 && !isJumping) {
            isJumpCut = false;
            isJumpFalling = false;
        }

        #endregion

    }

    private void FixedUpdate() {

        #region CHAMPION ACTIONS PLAYBACK
        if (!LoopManager.Instance.IsRecording || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) {
            HandleMovement(championActionsThisFrame.moveDir);
            if (championActionsThisFrame.JumpPressed) {
                JumpPressed();
            }
            if (championActionsThisFrame.JumpReleased) {
                JumpReleased();
            }
        }

        #endregion

        #region MOVEMENT
        if (LoopManager.Instance.IsRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            HandleMovement(moveInput);
        }
        #endregion

        #region JUMP

        if (lastGroundedTime > 0 && lastPressedJumpTime > 0 && !isJumping) {
            Jump();
        }

        #endregion

        #region GRAVITY
        

        // Higher gravity if jump button released and speed reduction
        if (isJumpCut) {
            SetGravityScale(championSO.gravityScale * championSO.jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -championSO.maxFallSpeed));
        } else {
            // Default gravity if grounded
            SetGravityScale(championSO.gravityScale);
        }
        #endregion
    }

    public void SetGravityScale(float scale) {
        rb.gravityScale = scale;
    }

    private void HandleMovement(float moveInput) {
        #region Movement

        // targetSpeet = speed to reach
        float targetSpeed = moveInput * championSO.moveSpeed;
        // calculate speed Diff between current speed and target speed
        float speedDiff = targetSpeed - rb.velocity.x;
        // acceleration or deceleration in regards to the situation (have we reached our targetspeed?)
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? championSO.acceleration: championSO.deceleration;
        // calculate force to add to rb
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, championSO.velPower) * Mathf.Sign(speedDiff);

        rb.AddForce(movement * Vector2.right);
        #endregion

        #region Friction
        // Check if we are grounded and we are trying to stop
        if (isGrounded && Mathf.Abs(moveInput) < 0.01f) {

            // Then we use either friction amount or velocity 
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(championSO.frictionAmount));
            // Set to movement direction
            amount *= Mathf.Sign(rb.velocity.x);

            // Apply force against movement direction
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion

    }

    private void Jump() {
        // apply force
        lastGroundedTime = 0;
        lastPressedJumpTime = 0;
        isJumping = true;
        isJumpCut = false;
        rb.AddForce(Vector2.up * championSO.jumpForce, ForceMode2D.Impulse);
    }

    private void JumpReleased() {
        isJumpCut = true;
    }

    private void JumpPressed() {
        isJumpCut = false;
        lastPressedJumpTime = championSO.jumpInputBufferTime;
    }

    private void InputManager_OnJumpPressed(object sender, System.EventArgs e) {
        if (LoopManager.Instance.IsRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            JumpPressed();
        }
    }

    private void InputManager_OnJumpReleased(object sender, System.EventArgs e) {
        if (LoopManager.Instance.IsRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            JumpReleased();
        }
    }

    public void SetChampionActionsThisFrame(ChampionActions championActions) {
        championActionsThisFrame = championActions;
    }

}
