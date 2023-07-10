using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int playerScore = 0;
    private int playerMana = 0;

    private int playerScoreOnRecordingStart;
    private int playerManaOnRecordingStart;

    public event EventHandler OnManaChanged;
    public event EventHandler OnScoreChanged;
    public int PlayerMana { get { return playerMana; } }
    public int PlayerScore { get {  return playerScore; } }

    public void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Recording) {
            playerScoreOnRecordingStart = playerScore;
            playerManaOnRecordingStart = playerMana;

            OnScoreChanged?.Invoke(this, EventArgs.Empty);
            OnManaChanged?.Invoke(this, EventArgs.Empty);
        }
        if (e.state == LoopManager.State.Playbacking) {
            playerScore = playerScoreOnRecordingStart;
            playerMana = playerManaOnRecordingStart;

            OnScoreChanged?.Invoke(this, EventArgs.Empty);
            OnManaChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void IncreaseScore(int score) {
        playerScore += score;
        OnScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseMana(int manaValue) {
        playerMana += manaValue;
        OnManaChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DecreaseMana(int manaValue) {
        playerMana -= manaValue;
        OnManaChanged?.Invoke(this, EventArgs.Empty);
    }
}
