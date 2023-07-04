using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionWeapon : MonoBehaviour
{

    private Champion champion;
    private ChampionSO championSO;

    private void Awake() {
        champion = GetComponent<Champion>();
    }

    private void Start() {
        championSO = champion.ChampionSO;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Champion") {
            collider.gameObject.GetComponent<KnockbackFeedback>().MeleeKnockback(this.gameObject);
            collider.gameObject.GetComponent<Champion>().ReceiveDamage(championSO.championAttackDamage);
        }
    }

    }
