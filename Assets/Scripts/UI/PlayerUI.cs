using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] GameObject[] manaOrbsImages;
    [SerializeField] GameObject[] scoreFlagImages;
    private Player player;

    private void Awake() {
        player = GetComponentInParent<Player>();
    }

    private void Start() {
        player.OnManaChanged += Player_OnManaChanged;
        player.OnScoreChanged += Player_OnScoreChanged;
    }

    private void Player_OnScoreChanged(object sender, System.EventArgs e) {
        int playerScore = player.PlayerScore;
        int i = 0;

        foreach (GameObject scoreFlagImage in scoreFlagImages) {
            if (i < playerScore) {
                scoreFlagImage.SetActive(true);
            }
            i++;
        }
    }

    private void Player_OnManaChanged(object sender, System.EventArgs e) {
        int playerMana = player.PlayerMana;
        int i = 0;

        foreach(GameObject manaOrbImage in manaOrbsImages) {
            if (i < playerMana) {
                manaOrbImage.SetActive(true);
            } else {
                manaOrbImage.SetActive(false);
            }
            i++;
        }
    }
}
