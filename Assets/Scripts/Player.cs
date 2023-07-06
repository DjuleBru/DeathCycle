using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int playerScore = 0;
    private int playerMana = 0;

    public int PlayerMana { get { return playerMana; } }
    public int PlayerScore { get {  return playerScore; } }

    public void IncreaseScore(int score) {
        playerScore += score;
        Debug.Log(playerScore);
    }

    public void IncreaseMana(int manaValue) {
        playerMana += manaValue;
        Debug.Log(playerMana);
    }

    public void DecreaseMana(int manaValue) {
        playerMana -= manaValue;
        Debug.Log(playerMana);
    }
}
