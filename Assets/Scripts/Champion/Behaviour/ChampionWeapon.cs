using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionWeapon : MonoBehaviour
{
    private ChampionAttackType championAttackType = new ChampionAttackType();

    private Champion champion;
    private IChampionAttack iChampionAttack;
    private IChampionSpecial iChampionSpecial;

    private float attackDamage;

    private void Awake() {
        champion = GetComponent<Champion>();
        iChampionAttack = GetComponent<IChampionAttack>();
        iChampionSpecial = GetComponent<IChampionSpecial>();

        iChampionAttack.OnAttack += ChampionAttack_OnAttack;
        iChampionSpecial.OnSpecial += IChampionSpecial_OnSpecial;
    }

    private void Start() {
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.tag == "Champion") {
            if (collider.gameObject.GetComponent<KnockbackFeedback>() != null) {
                collider.gameObject.GetComponent<KnockbackFeedback>().MeleeKnockback(this.gameObject);
            }
            collider.gameObject.GetComponent<Champion>().ReceiveDamage(attackDamage);
        }
    }

    private void ChampionAttack_OnAttack(object sender, IChampionAttack.OnAttackEventArgs e) {
        GetAttackDamage(e.attackType.ToString());
    }

    private void IChampionSpecial_OnSpecial(object sender, IChampionSpecial.OnSpecialEventArgs e) {
        GetAttackDamage(e.attackType.ToString());
    }

    private void GetAttackDamage(string attackType) {
        if (attackType == championAttackType.AIRATTACK) {
            attackDamage = champion.ChampionSO.championAirAttackDamage;
        }
        if (attackType == championAttackType.ATTACK1) {
            attackDamage = champion.ChampionSO.championAttack1Damage;
        }
        if (attackType == championAttackType.ATTACK2) {
            attackDamage = champion.ChampionSO.championAttack2Damage;
        }
        if (attackType == championAttackType.ATTACK3) {
            attackDamage = champion.ChampionSO.championAttack3Damage;
        }
        if (attackType == championAttackType.SPECIALATTACK) {
            attackDamage = champion.ChampionSO.championSpecialDamage;
        }
    }

}
