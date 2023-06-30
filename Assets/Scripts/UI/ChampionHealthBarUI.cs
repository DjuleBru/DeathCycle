using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionHealthBarUI : MonoBehaviour
{
    [SerializeField] Image healthBar;
    private Champion champion;

    private float healthBarDisplayTime = 1.5f;
    private float healthBarDisplayTimer;
    private float championMaxHealth;

    private void Awake() {
        champion = GetComponentInParent<Champion>();
        champion.OnDamageReceived += Champion_OnDamageReceived;
    }


    private void Start() {
        championMaxHealth = champion.ChampionSO.championMaxHealth;
        Hide();
    }

    private void Update() {
        healthBar.fillAmount = champion.GetChampionHealth() / championMaxHealth;

        if (healthBar.fillAmount == 0) {
            Hide();
        }

        if (healthBarDisplayTimer  > 0) {
            Show();
            healthBarDisplayTimer -= Time.deltaTime;
        } else {
            Hide();
        }
    }

    private void Champion_OnDamageReceived(object sender, Champion.OnDamageReceivedEventArgs e) {
        Show();
        healthBarDisplayTimer = healthBarDisplayTime;
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
