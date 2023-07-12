using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoopManagerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pauseCountDownText;
    [SerializeField] TextMeshProUGUI recplayCountDownText;
    [SerializeField] TextMeshProUGUI LoopNumberText;
    [SerializeField] Image RecordingImage;
    [SerializeField] Image PlaybackImage;

    private float loopOnPauseTimer;
    private float loopOnPauseTime;
    private float loopTime;
    private float loopTimer;
    private int loopNumberDisplayed = 1;

    private bool loopOnPause;
    private bool loopOnRecording;
    private bool loopOnPlaybacking;
    private bool loopOnEndBuffer;

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;

        loopOnPauseTime = LoopManager.Instance.LoopPauseTime;
        loopTime = LoopManager.Instance.LoopTime;
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Pause) {

            loopOnPause = true;
            loopOnRecording = false;
            loopOnPlaybacking = false;
            loopOnEndBuffer = false;

            loopOnPauseTimer = loopOnPauseTime;

            RecordingImage.gameObject.SetActive(false);
            PlaybackImage.gameObject.SetActive(false);

            recplayCountDownText.gameObject.SetActive(false);
            LoopNumberText.gameObject.SetActive(true);
            pauseCountDownText.gameObject.SetActive(true);

            LoopNumberText.text = "Loop " + loopNumberDisplayed;
        }
        if (e.state == LoopManager.State.Recording) {
            loopOnPause = false;
            loopOnRecording = true;
            loopOnPlaybacking = false;
            loopOnEndBuffer = false;

            loopTimer = loopTime;

            RecordingImage.gameObject.SetActive(true);
            PlaybackImage.gameObject.SetActive(false);

            recplayCountDownText.gameObject.SetActive(true);
            LoopNumberText.gameObject.SetActive(false);
            pauseCountDownText.gameObject.SetActive(false);
        }
        if (e.state == LoopManager.State.Playbacking) {
            loopOnPause = false;
            loopOnRecording = false;
            loopOnPlaybacking = true;
            loopOnEndBuffer = false;

            loopTimer = loopTime;

            RecordingImage.gameObject.SetActive(false);
            PlaybackImage.gameObject.SetActive(true);

            recplayCountDownText.gameObject.SetActive(true);
            LoopNumberText.gameObject.SetActive(false);
            pauseCountDownText.gameObject.SetActive(false);
            loopNumberDisplayed++;
        }
        if (e.state == LoopManager.State.RecordingEndBuffer || e.state == LoopManager.State.PlaybackEndBuffer) {
            loopOnEndBuffer = true;
        }
    }

    private void Update() {
        if (loopOnPause) {
            pauseCountDownText.text = Math.Ceiling(loopOnPauseTimer).ToString();
            loopOnPauseTimer -= Time.deltaTime;
        }
        if (loopOnRecording || loopOnPlaybacking) {
            recplayCountDownText.text = Math.Ceiling(loopTimer).ToString();
            loopTimer -= Time.deltaTime;
        }
        if (loopOnEndBuffer) {
            recplayCountDownText.text = "0";
        }
    }

}
