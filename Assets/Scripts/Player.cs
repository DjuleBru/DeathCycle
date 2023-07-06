using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int playerScore = 0;

    public void increaseScore(int score) {
        playerScore += score;
        Debug.Log(playerScore);
    }
}
