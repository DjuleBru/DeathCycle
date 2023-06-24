using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour {

    // Dictionaries relative to this loop's player data
    private Dictionary<int, Vector3> playerPositionRecord = new Dictionary<int, Vector3>();
    private Dictionary<int, bool> playerIsJump = new Dictionary<int, bool>();
    private Dictionary<int, bool> playerIsAttack = new Dictionary<int, bool>();
    private Dictionary<int, bool> playerIsSpecial = new Dictionary<int, bool>();
    private Dictionary<int, bool> playerIsIdle = new Dictionary<int, bool>();

    private Champion player;
    private Vector3 playerPosition;

    private List<PlayerActions> playerActions = new List<PlayerActions>();
    private List<PlayerActions> playerActionsPlayback = new List<PlayerActions>();

    private float loopTime = 100f;
    private float loopTimer = 0f;
    private int loopIndex = 0;

    private bool isRecording = true;
    public bool IsRecording { get { return isRecording; } }

    public void Start() {
        player = GetComponent<Champion>();
    }

    void Update() {
        if (isRecording) {
            Recording();
        } else {
            PlayBack();
        }

    }

    void Recording() {
        if (loopTimer <= loopTime) {

            playerPosition = player.GetPlayerPosition();
            playerPositionRecord.Add(loopIndex, playerPosition);

            playerActions = player.GetPlayerActions();
            HandlePlayerActionsRecord(loopIndex, playerActions);

            loopTimer += Time.deltaTime;
            loopIndex++;
        } else {
            isRecording = false;
            loopTimer = 0;
            loopIndex = 0;
        }
    }

    void PlayBack() {
        if (loopIndex < playerPositionRecord.Count) {

            playerPosition = playerPositionRecord[loopIndex];
            playerActionsPlayback = HandlePlayerActionsPlayBack(loopIndex);

            player.SetPlayerposition(playerPosition);
            player.HandleActions(playerActionsPlayback);
            playerActionsPlayback.Clear();

            loopTimer += Time.deltaTime;
            loopIndex++;
        }
    }

    void HandlePlayerActionsRecord(int loopIndex, List<PlayerActions> playerActions) {
        foreach (PlayerActions playerAction in playerActions) {
            if (playerAction == PlayerActions.attack) {
                playerIsAttack.Add(loopIndex, true);
            }
            if (playerAction == PlayerActions.special) {
                playerIsSpecial.Add(loopIndex, true);
            }
            if (playerAction == PlayerActions.jump) {
                playerIsJump.Add(loopIndex, true);
            }
            if (playerAction == PlayerActions.idle) {
                playerIsIdle.Add(loopIndex, true);
            }
        }
    }

    List<PlayerActions> HandlePlayerActionsPlayBack(int loopIndex) {
        if(playerIsAttack.ContainsKey(loopIndex)) {
            playerActionsPlayback.Add(PlayerActions.attack);
            Debug.Log("PlayerActions.attack");
        }
        if (playerIsJump.ContainsKey(loopIndex)) {
            playerActionsPlayback.Add(PlayerActions.jump);
            Debug.Log("PlayerActions.jump");
        }
        if (playerIsSpecial.ContainsKey(loopIndex)) {
            playerActionsPlayback.Add(PlayerActions.special);
            Debug.Log("PlayerActions.Special");
        }
        if (playerIsIdle.ContainsKey(loopIndex)) {
            playerActionsPlayback.Add(PlayerActions.idle);
            Debug.Log("PlayerActions.Idle");
        }
        return playerActionsPlayback;
    }
}
