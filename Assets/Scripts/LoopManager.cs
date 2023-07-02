using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class LoopManager : MonoBehaviour {

    public static LoopManager Instance { get; private set; }

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State state;
        public int loopNumber;
    }

    public enum State {
        Pause,
        Recording,
        Playbacking,
    }

    private State state;

    private float loopPauseTime = 1f;
    private float loopTime = 5f;
    private float loopRecordingTimer = 0f;
    private float loopPlaybackTimer = 0f;
    private float loopPauseTimer = 0f;

    private int loopNumber = 0;

    public int LoopNumber { get { return loopNumber; } }
    public float LoopPauseTime { get { return loopPauseTime; } }
    public float LoopTime { get { return loopTime; } }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one LoopManager Instance");
        }
        Instance = this;
    }

    private void Start() {
        state = State.Pause;
    }

    private void Update() {

        switch (state) {
            case State.Pause:

                if (loopPauseTimer <= loopPauseTime) {
                    loopPauseTimer += Time.deltaTime;
                } else {
                    state = State.Recording;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        loopNumber = loopNumber,
                        state = state
                    });
                    loopRecordingTimer = 0f;
                }

                break;
            case State.Recording:

                if (loopRecordingTimer <= loopTime) {
                    loopRecordingTimer += Time.deltaTime;
                } else {
                    state = State.Playbacking;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        loopNumber = loopNumber,
                        state = state
                    });
                    loopPlaybackTimer = 0f;
                }

                break;
            case State.Playbacking:

                if (loopPlaybackTimer <= loopTime) {
                    loopPlaybackTimer += Time.deltaTime;
                } else {
                    state = State.Pause;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        loopNumber = loopNumber,
                        state = state
                    });

                    loopPauseTimer = 0f;
                    loopNumber++;
                }
                break;
        }
    }

}