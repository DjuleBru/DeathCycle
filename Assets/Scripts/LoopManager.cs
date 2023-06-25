using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour {
<<<<<<< Updated upstream

    // Dictionaries relative to this loop's player data
    private Dictionary<int, Vector3> playerPositionRecord = new Dictionary<int, Vector3>();
    private Dictionary<int, bool> playerIsJump = new Dictionary<int, bool>();
    private Dictionary<int, bool> playerIsAttack = new Dictionary<int, bool>();
    private Dictionary<int, bool> playerIsSpecial = new Dictionary<int, bool>();
    private Dictionary<int, bool> playerIsIdle = new Dictionary<int, bool>();

    private Player player;
    private Vector3 playerPosition;

    private List<PlayerActions> playerActions = new List<PlayerActions>();
    private List<PlayerActions> playerActionsPlayback = new List<PlayerActions>();

    private float loopTime = 3f;
    private float loopTimer = 0f;
    private int loopIndex = 0;

    private bool isRecording = true;
    public bool IsRecording { get { return isRecording; } }

    public void Start() {
        player = GetComponent<Player>();
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
=======
    public static LoopManager Instance { get; private set; }

    private float loopTime = 2f;
    private float loopRecordingTimer = 0f;
    private float loopPlaybackTimer = 0f;

    private int loopNumber = 1;
    private bool isRecording;

    public bool IsRecording { get { return isRecording; } }
    public int LoopNumber { get { return loopNumber; } }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one LoopManager Instance");
        }
        Instance = this;
    }

    void Update() {
        Debug.Log(loopNumber);
        // 1: recording
        if (loopRecordingTimer <= loopTime) {
            isRecording = true;
            loopRecordingTimer += Time.deltaTime;
        }
        // 2: recording end
        else {
            // 3: playback 
            if (loopPlaybackTimer <= loopTime) {
                loopPlaybackTimer += Time.deltaTime;
                isRecording = false;
            } else {
                // 4: playback end
                loopRecordingTimer = 0;
                loopPlaybackTimer = 0;
                loopNumber++;
                Debug.Log("Loop n " + loopNumber);
            }
        }
>>>>>>> Stashed changes
    }
}