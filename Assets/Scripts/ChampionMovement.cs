using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChampionMovement : MonoBehaviour
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

    [SerializeField] private InputManager inputManager;


    private float lastGroundedTime;
    private float lastPressedJumpTime;
    private Rigidbody2D rb;
    private float moveInput;

    private bool isGrounded;
    private bool isJumping;
    private bool isJumpFalling;
    private bool jumpInputReleased;
    private bool isJumpCut;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    private void FixedUpdate() {
       // Debug.Log(isJumpCut);
        HandleMovement(moveInput);

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
    }
    private void InputManager_OnJumpReleased(object sender, System.EventArgs e) {
        JumpReleased();
    }
    public void SetGravityScale(float scale) {
        rb.gravityScale = scale;
    }
}
