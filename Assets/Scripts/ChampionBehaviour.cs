using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChampionBehaviour : MonoBehaviour
{

    #region PLAYER DATA
    [SerializeField] private float moveSpeed;
    [SerializeField] private float velPower;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float frictionAmount;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoyoteTime = 0.2f;
    [SerializeField] private float jumpInputBufferTime = 0.1f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float jumpCutGravityMult = 2f;
    [SerializeField] private float maxFallSpeed;

    #endregion

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

<<<<<<< Updated upstream:Assets/Scripts/ChampionBehaviour.cs
    private float moveInput;
=======
    #region ANIMATOR PARAMETERS
    private float moveDir;
    public float MoveDir { get { return moveDir; } }

    #endregion

    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        rb = GetComponent<Rigidbody2D>();
        champion = GetComponent<Champion>();
        championSO = champion.ChampionSO;
>>>>>>> Stashed changes:Assets/Scripts/Champion/Behaviour/ChampionMovement.cs

    private ChampionRecPlaybackManager loopManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private ChampionActions championActionsThisFrame = new ChampionActions();
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputManager.OnJumpPressed += InputManager_OnJumpPressed;
        inputManager.OnJumpReleased += InputManager_OnJumpReleased;
        loopManager = GetComponent<ChampionRecPlaybackManager>();
    }

    void Update()
    {
        #region TIMERS
        lastGroundedTime -= Time.deltaTime;
        lastPressedJumpTime -= Time.deltaTime;
        #endregion

        #region INPUTHANDLER
        moveInput = inputManager.GetMoveInput();
        #endregion

        #region COLLISION CHECKS
        if (!isJumping) {
            // Grounded check
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) {
                lastGroundedTime = jumpCoyoteTime;
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

        #region CHAMPION ACTIONS RECORDING
        if (LoopManager.Instance.IsRecording) {
            championActionsThisFrame.moveDir = moveInput;
        }
        #endregion
    }

    private void FixedUpdate() {
<<<<<<< Updated upstream:Assets/Scripts/ChampionBehaviour.cs
        HandleMovement(moveInput);
=======

        #region MOVEMENT
        if (loopOnRecording && LoopManager.Instance.LoopNumber == champion.SpawnedLoopNumber) {
            HandleMovement(moveInput);
            moveDir = moveInput;
        }
        if ((loopOnPlaybacking || LoopManager.Instance.LoopNumber != champion.SpawnedLoopNumber) && !loopOnPause) {
            HandleMovement(championActionsThisFrame.moveDir);
            moveDir = championActionsThisFrame.moveDir;
        }
            #endregion
>>>>>>> Stashed changes:Assets/Scripts/Champion/Behaviour/ChampionMovement.cs

        #region JUMP

        if(lastGroundedTime > 0 && lastPressedJumpTime > 0 && !isJumping) {
            Jump();
        }

        #endregion

        #region GRAVITY
        

        // Higher gravity if jump button released and speed reduction
        if (isJumpCut) {
            SetGravityScale(gravityScale * jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        } else {
            // Default gravity if grounded
            SetGravityScale(gravityScale);
        }
        #endregion

        #region CHAMPION ACTIONS PLAYBACK
        if (!LoopManager.Instance.IsRecording) {
            HandleMovement(championActionsThisFrame.moveDir);
            if (championActionsThisFrame.JumpPressed) {
                JumpPressed();
            }
            if (championActionsThisFrame.JumpReleased) {
                JumpReleased();
            }
        }
        #endregion
    }

    private void HandleMovement(float moveInput) {
        #region Movement

        // targetSpeet = speed to reach
        float targetSpeed = moveInput * moveSpeed;
        // calculate speed Diff between current speed and target speed
        float speedDiff = targetSpeed - rb.velocity.x;
        // acceleration or deceleration in regards to the situation (have we reached our targetspeed?)
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration: deceleration;
        // calculate force to add to rb
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

        rb.AddForce(movement * Vector2.right);
        #endregion

        #region Friction
        // Check if we are grounded and we are trying to stop
        if (isGrounded && Mathf.Abs(moveInput) < 0.01f) {

            // Then we use either friction amount or velocity 
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
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
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void JumpReleased() {
        isJumpCut = true;
    }

    private void JumpPressed() {
        isJumpCut = false;
        lastPressedJumpTime = jumpInputBufferTime;
    }

    private void InputManager_OnJumpPressed(object sender, System.EventArgs e) {
        JumpPressed();
        championActionsThisFrame.JumpPressed = true;
        championActionsThisFrame.JumpReleased = false;
    }
    private void InputManager_OnJumpReleased(object sender, System.EventArgs e) {
        JumpReleased();
        championActionsThisFrame.JumpReleased = true;
        championActionsThisFrame.JumpPressed = false;
    }
    public void SetGravityScale(float scale) {
        rb.gravityScale = scale;
    }

    public ChampionActions GetChampionActionsThisFrame() {
        return championActionsThisFrame;
    }

    public void SetChampionActionsThisFrame(ChampionActions championActions) {
        championActionsThisFrame = championActions;
    }
}