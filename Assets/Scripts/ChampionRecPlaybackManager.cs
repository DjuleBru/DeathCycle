using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionRecPlaybackManager : MonoBehaviour {

    // Dictionaries relative to this loop's player data
    private Dictionary<int, ChampionActions> championActionsRecord = new Dictionary<int, ChampionActions>();

    private Champion champion;
    private ChampionBehaviour championMovement;
    private ChampionActions championActions;

    private float loopTime = 3f;
    private float loopTimer = 0f;
    private int loopIndex = 0;

    private bool isRecording = true;
    public bool IsRecording { get { return isRecording; } }

    public void Start() {
        champion = GetComponent<Champion>();
        championMovement = GetComponent<ChampionBehaviour>();
    }

    void Update() {
        if (!isRecording) {
            PlayBack();
        }
           
    }

    private void LateUpdate() {
        if (isRecording) {
            Recording();
        }
    }

    void Recording() {
        if (loopTimer <= loopTime) {

            championActions = championMovement.GetChampionActionsThisFrame();

            championActionsRecord[loopIndex] = new ChampionActions {
                Attack = championActions.Attack,
                moveDir = championActions.moveDir, 
                JumpPressed = championActions.JumpPressed,
                JumpReleased = championActions.JumpReleased,
                Special = championActions.Special,
            };

            loopTimer += Time.deltaTime;
            loopIndex++;
        } else {
            isRecording = false;
            loopTimer = 0;
            loopIndex = 0;
        }
    }

    void PlayBack() {
        if (loopIndex < championActionsRecord.Count) {

            championActions = championActionsRecord[loopIndex];

            championMovement.SetChampionActionsThisFrame(championActions);

            loopTimer += Time.deltaTime;
            loopIndex++;
        }
    }
}
