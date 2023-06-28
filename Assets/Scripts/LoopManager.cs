using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour {
    public static LoopManager Instance { get; private set; }

    public event EventHandler<OnRecordingEventArgs> OnRecordingStarted;
    public event EventHandler<OnRecordingEventArgs> OnPlaybackStarted;

    public class OnRecordingEventArgs : EventArgs{
        public int loopNumber;
    };

    private float loopTime = 3f;
    private float loopRecordingTimer = 0f;
    private float loopPlaybackTimer = 0f;

    private int loopNumber = 0;
    private bool isRecording;

    public bool IsRecording { get { return isRecording; } }
    public int LoopNumber { get { return loopNumber; } }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one LoopManager Instance");
        }
        Instance = this;
    }

    void FixedUpdate() {
        // 1: recording
        if (loopRecordingTimer == 0) {
            OnRecordingStarted?.Invoke(this, new OnRecordingEventArgs {
                loopNumber = loopNumber
            });
        }
        if (loopRecordingTimer <= loopTime) {
            isRecording = true;
            loopRecordingTimer += Time.deltaTime;
        }
        // 2: recording end
        else {
            if (loopPlaybackTimer == 0) {
                OnPlaybackStarted?.Invoke(this, new OnRecordingEventArgs {
                    loopNumber = loopNumber
                });
            }

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