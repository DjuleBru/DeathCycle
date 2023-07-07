using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionRecPlaybackManager : MonoBehaviour {

    // Dictionary relative to this loop's player data
    private Dictionary<int, ChampionActions> championActionsRecord = new Dictionary<int, ChampionActions>();

    private InputRecorder inputRecorder;
    private ChampionMovement championMovement;
    private IChampionAttack IChampionAttack;
    private IChampionSpecial IChampionSpecial;
    private ChampionActions championActions;
    private Champion champion;

    public enum State {
        Pause,
        Recording,
        Playbacking,
    }

    private State loopManagerState;

    private int recordingLoopIndex = 0;
    private int playbackLoopIndex = 0;

    private float loopTimer = 0f;

    private void Awake() {
        inputRecorder = FindObjectOfType<InputRecorder>();
        championMovement = GetComponent<ChampionMovement>();
        IChampionAttack = GetComponent<IChampionAttack>();
        IChampionSpecial = GetComponent<IChampionSpecial>();
        champion = GetComponent<Champion>();

        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Pause) {
            loopManagerState = State.Pause;
        }
        if (e.state == LoopManager.State.Recording) {
            loopManagerState = State.Recording;
            playbackLoopIndex = 0;
            loopTimer = 0f;
        }
        if (e.state == LoopManager.State.Playbacking) {
            loopManagerState = State.Playbacking;
            playbackLoopIndex = 0;
            loopTimer = 0f;
        }
    }

    private void Update() {
        if (loopManagerState == State.Recording && champion.SpawnedLoopNumber == LoopManager.Instance.LoopNumber) {
            loopTimer += Time.deltaTime;
            Recording();
        }

        if (loopManagerState == State.Playbacking || (loopManagerState == State.Recording && champion.SpawnedLoopNumber != LoopManager.Instance.LoopNumber)) {
            loopTimer += Time.deltaTime;
            PlayBack();
        }
    }

    void Recording() {
            championActions = inputRecorder.GetChampionActionsThisFrame();

            championActionsRecord[recordingLoopIndex] = new ChampionActions {
                actionTimeInLoop = loopTimer,
                AttackPressed = championActions.AttackPressed,
                AttackReleased = championActions.AttackReleased,
                mousePos = championActions.mousePos,
                moveDir = championActions.moveDir,
                JumpPressed = championActions.JumpPressed,
                JumpReleased = championActions.JumpReleased,
                SpecialPressed = championActions.SpecialPressed,
                SpecialReleased = championActions.SpecialReleased,
            };

        Debug.Log("loopIndex " + recordingLoopIndex + " time " + loopTimer + " " + championActionsRecord[recordingLoopIndex].AttackPressed);
        
        recordingLoopIndex++;

    }

    void PlayBack() {
        if (playbackLoopIndex < championActionsRecord.Count) {

            championActions = championActionsRecord[playbackLoopIndex];

            while (championActions.actionTimeInLoop <=  loopTimer) {
                playbackLoopIndex++;

                if (playbackLoopIndex < championActionsRecord.Count) {
                    championActions = championActionsRecord[playbackLoopIndex];
                } else { return; }
            }

            Debug.Log("loopIndex " + playbackLoopIndex + " time " + loopTimer + " " + championActionsRecord[playbackLoopIndex].AttackPressed);

            championMovement.SetChampionActionsThisFrame(championActions);
            IChampionAttack.SetChampionActionsThisFrame(championActions);
            IChampionSpecial.SetChampionActionsThisFrame(championActions);

        }
    }
}
