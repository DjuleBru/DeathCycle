using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionRecPlaybackManager : MonoBehaviour {

    // Dictionary relative to this loop's player data
    private Dictionary<int, ChampionActions> championActionsRecord = new Dictionary<int, ChampionActions>();

    private ChampionBehaviour championBehaviour;
    private ChampionActions championActions;
    private Champion champion;

    private int recordingLoopIndex = 0;
    private int playbackLoopIndex = 0;

    public void Start() {
        championBehaviour = GetComponent<ChampionBehaviour>();
        champion = GetComponent<Champion>();
        LoopManager.Instance.OnRecordingStarted += LoopManager_OnRecordingStarted;
        LoopManager.Instance.OnPlaybackStarted += LoopManager_OnPlaybackStarted;
    }


    private void FixedUpdate() {
        if (!LoopManager.Instance.IsRecording) {
            // Loop is playbacking
            PlayBack();
        } else { 
            // Loop is recording
            if (champion.SpawnedLoopNumber == LoopManager.Instance.LoopNumber) {
                // Loop Number set on Spawn = Active loop number
                Recording();
            } else {
                PlayBack();
            }
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
            Debug.Log(LoopManager.Instance.LoopNumber + " " + playbackLoopIndex + " " + championActionsRecord[playbackLoopIndex].moveDir);

            playbackLoopIndex++;
        }
    }
    private void LoopManager_OnRecordingStarted(object sender, LoopManager.OnRecordingEventArgs e) {
        playbackLoopIndex = 0;
    }

    private void LoopManager_OnPlaybackStarted(object sender, LoopManager.OnRecordingEventArgs e) {
        playbackLoopIndex = 0;
    }
}
