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

    private ChampionRecPlaybackManager loopManager;
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
    private float lastPressedJumpTime = 0f;
    #endregion

    #region BOOL CHECKERS
    private bool isGrounded;
    private bool isJumping;
    private bool isJumpCut;

    private bool loopOnPause;
    private bool loopOnRecording;
    private bool loopOnPlaybacking;
    #endregion

    #region ANIMATOR PARAMETERS
    private float moveDir;
    public float MoveDir { get { return moveDir; } }

    #endregion


    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody2D>();
        champion = GetComponent<Champion>();
        championSO = champion.ChampionSO;

        inputManager.OnJumpPressed += InputManager_OnJumpPressed;
        inputManager.OnJumpReleased += InputManager_OnJumpReleased;
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
    }

    void Update() {
        #region TIMERS
        lastGroundedTime -= Time.deltaTime;
        lastPressedJumpTime -= Time.deltaTime;
        #endregion

        #region INPUTHANDLER
        if (loopOnRecording) {
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
        }

        if (lastGroundedTime > 0 && !isJumping) {
            isJumpCut = false;
        }

        #endregion

        #region CHAMPION JUMP INPUT PLAYBACK
        if (loopOnPlaybacking || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) {
            if (championActionsThisFrame.JumpPressed) {
                JumpPressed();
            }
            if (championActionsThisFrame.JumpReleased) {
                JumpReleased();
            }
        }
        #endregion
    }

    private void FixedUpdate() {

        #region MOVEMENT
        if (loopOnRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            moveDir = moveInput;
            HandleMovement(moveDir);
        }
        if ((loopOnPlaybacking || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) && !loopOnPause) {
            moveDir = championActionsThisFrame.moveDir;
            HandleMovement(moveDir);
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
        if (loopOnRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            JumpPressed();
        }
    }

    private void InputManager_OnJumpReleased(object sender, System.EventArgs e) {
        if (loopOnRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            JumpReleased();
        }
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        rb.velocity = Vector3.zero;

        Debug.Log(e.state.ToString());
        if (e.state == LoopManager.State.Pause) {
            loopOnPause = true;
            loopOnRecording = false;
            loopOnPlaybacking = false;
        }
        if (e.state == LoopManager.State.Recording) {
            loopOnPause = false;
            loopOnRecording = true;
            loopOnPlaybacking = false;
        }
        if (e.state == LoopManager.State.Playbacking) {
            loopOnPause = false;
            loopOnRecording = false;
            loopOnPlaybacking = true;
        }
    }

    public void SetChampionActionsThisFrame(ChampionActions championActions) {
        championActionsThisFrame = championActions;
    }
}

