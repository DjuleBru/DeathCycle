using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int playerScore = 0;
    private int playerMana = 0;

    public event EventHandler OnManaChanged;
    public event EventHandler OnScoreChanged;
    public int PlayerMana { get { return playerMana; } }
    public int PlayerScore { get {  return playerScore; } }

    public void IncreaseScore(int score) {
        playerScore += score;
        OnScoreChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log(playerScore);
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
