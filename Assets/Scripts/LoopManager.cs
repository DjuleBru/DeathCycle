using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour {
    public static LoopManager Instance { get; private set; }

    private float loopTime = 2f;
    private float loopRecordingTimer = 0f;
    private float loopPlaybackTimer = 0f;

    private int loopNumber = 1;
    private bool isRecording;

    public bool IsRecording { get { return isRecording; } }
    public int LoopNumber { get { return loopNumber;  } }

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
            }
        }
    }
}
