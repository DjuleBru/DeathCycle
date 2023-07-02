using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionRecPlaybackManager : MonoBehaviour {

    // Dictionary relative to this loop's player data
    private Dictionary<int, ChampionActions> championActionsRecord = new Dictionary<int, ChampionActions>();

    private ChampionBehaviour championBehaviour;
    private ChampionActions championActions;

    private int recordingLoopIndex = 0;
    private int playbackLoopIndex = 0;

    public void Start() {
        championBehaviour = GetComponent<ChampionBehaviour>();
    }

    void Update() {
        if (!LoopManager.Instance.IsRecording) {
            PlayBack();
        }

    }

    private void LateUpdate() {
        if (LoopManager.Instance.IsRecording) {
            Recording();
        }
    }

    void Recording() {
        championActions = championBehaviour.GetChampionActionsThisFrame();

        championActionsRecord[recordingLoopIndex] = new ChampionActions {
            Attack = championActions.Attack,
            moveDir = championActions.moveDir,
            JumpPressed = championActions.JumpPressed,
            JumpReleased = championActions.JumpReleased,
            Special = championActions.Special,
        };
        recordingLoopIndex++;
    }

    void PlayBack() {
        if (playbackLoopIndex < championActionsRecord.Count) {

            championActions = championActionsRecord[playbackLoopIndex];

            championBehaviour.SetChampionActionsThisFrame(championActions);

            playbackLoopIndex++;
        }
    }
}
