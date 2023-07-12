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

    private ChampionSO championSO;
    private Champion champion;
    private Rigidbody2D rb;

    private IChampionSpecial iChampionSpecial;
    private IChampionAttack iChampionAttack;

    #region CHECK PARAMETERS
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

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
    private bool loopOnEndBuffer;

    private bool isAttacking;
    private bool isSpecialing;
    #endregion

    #region ANIMATOR PARAMETERS
    private float moveDir;
    private float moveInput;
    public float MoveDir { get { return moveDir; } }
    public float MoveInput { get { return moveInput; } }
    public bool IsGrounded { get { return isGrounded; } }

    #endregion


    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody2D>();
        champion = GetComponent<Champion>();
        iChampionSpecial = GetComponent<IChampionSpecial>();
        iChampionAttack = GetComponent<IChampionAttack>();
        championSO = champion.ChampionSO;

        _groundCheckSize.x = _frontWallCheckPoint.transform.position.x;

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
        if (loopOnRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            moveInput = inputManager.GetMoveInput();
            moveDir = moveInput;
        }
        if ((loopOnPlaybacking || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) && !loopOnPause && !loopOnEndBuffer) {
            moveDir = championActionsThisFrame.moveDir;
        }
        if (loopOnEndBuffer || loopOnPause || iChampionAttack.IsAttacking || iChampionSpecial.IsSpecialing) {
            moveDir = 0f;
        }
        #endregion

        #region COLLISION CHECKS
        // Grounded check
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) {
            lastGroundedTime = championSO.jumpCoyoteTime;
            isGrounded = true;
        } else {
            isGrounded = false;
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
        HandleMovement(moveDir);
        #endregion

        #region JUMP

        if (lastGroundedTime > 0 && lastPressedJumpTime > 0 && !isJumping && !iChampionAttack.IsAttacking && !iChampionSpecial.IsSpecialing) {
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

        // set visual left or right
        if (moveInput != 0) {
            if (moveInput > 0) {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            } else {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

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
        if (e.state == LoopManager.State.Pause) {
            loopOnPause = true;
            loopOnRecording = false;
            loopOnPlaybacking = false;
            loopOnEndBuffer = false;
        }
        if (e.state == LoopManager.State.Recording) {
            loopOnPause = false;
            loopOnRecording = true;
            loopOnPlaybacking = false;
            loopOnEndBuffer = false;
        }
        if (e.state == LoopManager.State.Playbacking) {
            loopOnPause = false;
            loopOnRecording = false;
            loopOnPlaybacking = true;
            loopOnEndBuffer = false;
        }
        if (e.state == LoopManager.State.PlaybackEndBuffer || e.state == LoopManager.State.RecordingEndBuffer) {
            loopOnPause = false;
            loopOnRecording = false;
            loopOnPlaybacking = false;
            loopOnEndBuffer = true;
        }
    }

    public void SetVelocity(Vector3 velocity) {
        rb.velocity = velocity;
    }

    public void IsAttacking(bool isAttacking) {
        this.isAttacking = isAttacking;
    }

    public void IsSpecialing(bool isSpecialing) {
        this.isSpecialing = isSpecialing;
    }

    public void SetChampionActionsThisFrame(ChampionActions championActions) {
        championActionsThisFrame = championActions;
    }
}

