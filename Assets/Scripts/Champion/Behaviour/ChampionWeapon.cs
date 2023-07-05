using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionWeapon : MonoBehaviour
{

    private Champion champion;
    private IChampionAttack iChampionAttack;

    private float attackDamage;


    private void Awake() {
        champion = GetComponent<Champion>();
        iChampionAttack = GetComponent<IChampionAttack>();

        iChampionAttack.OnAttack += ChampionAttack_OnAttack;
    }

    private void Start() {
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "Champion") {
            collider.gameObject.GetComponent<KnockbackFeedback>().MeleeKnockback(this.gameObject);
            collider.gameObject.GetComponent<Champion>().ReceiveDamage(attackDamage);
        }
    }

    private void ChampionAttack_OnAttack(object sender, IChampionAttack.OnAttackEventArgs e) {
        GetAttackDamage(e.attackCount);
    }

    private void GetAttackDamage(int attackCount) {
        if (attackCount == 0) {
            attackDamage = champion.ChampionSO.championAirAttackDamage;
        }
        if (attackCount == 1) {
            attackDamage = champion.ChampionSO.championAttack1Damage;
        }
        if (attackCount == 2) {
            attackDamage = champion.ChampionSO.championAttack2Damage;
        }
        if (attackCount == 3) {
            attackDamage = champion.ChampionSO.championAttack3Damage;
        }
    }

}
