using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActions {
    idle,
    jump,
    attack,
    special,
}

public class Champion : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private LoopManager loopManager;

    private Rigidbody2D rb;
    private ChampionMovement championMovement;
    private ParticleSystem Specialps;
    private List<PlayerActions> playerActions = new List<PlayerActions>();

    private void Start() {
        loopManager = GetComponent<LoopManager>();
        Specialps = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        championMovement = GetComponent<ChampionMovement>();

    }
    void Update()
    {
        if (loopManager.IsRecording) {
            HandleMovement();
            HandleInputActions();
        }
    }

    private void LateUpdate() {
        playerActions.Clear();
    }

    public void HandleMovement() {
        Vector2 inputVector = Vector2.zero;

        if (Input.GetKey(KeyCode.Q)) {
            inputVector += Vector2.left;
        }

        if (Input.GetKey(KeyCode.D)) {
            inputVector += Vector2.right;
        }

        rb.velocity = inputVector * moveSpeed * Time.deltaTime;
    }

    private void HandleInputActions() {
        if (Input.GetKey(KeyCode.E)) {
            playerActions.Add(PlayerActions.attack);
        }
        if (Input.GetKey(KeyCode.F)) {
            playerActions.Add(PlayerActions.special);
        }
        if (Input.GetKey(KeyCode.Space)) {
            playerActions.Add(PlayerActions.jump);
        }
        if (playerActions.Count == 0) {
            playerActions.Add(PlayerActions.idle);
        }
        HandleActions(playerActions);
    }

    public void HandleActions(List<PlayerActions> playerAction) {
        foreach (PlayerActions Action in playerAction) {
            if (Action == PlayerActions.attack) {
                Attack();
            }
            if (Action == PlayerActions.special) {
                Special();
            }
            if (Action == PlayerActions.idle) {
                Idle();
            }
        }
    }

    private void Special() {
        Specialps.gameObject.SetActive(true);
    }

    private void Attack() {
    }
    
    private void Idle() {
        Specialps.gameObject.SetActive(false);
    }

    public Vector3 GetPlayerPosition() {
        return transform.position;
    }

    public List<PlayerActions> GetPlayerActions() {
        return playerActions;
    }

    public void SetPlayerposition(Vector3 playerPosition) {
        this.transform.position = playerPosition;
    }

}
