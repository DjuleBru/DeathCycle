using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChampionMeleeAttack : MonoBehaviour
{

    private ChampionAimMelee championAimMelee;
    private Champion champion;
    private ChampionSO championSO;

    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    private float championAttackDamage;

    public LayerMask championLayer;

    private void Awake() {
        champion = GetComponent<Champion>();
        championAimMelee = champion.GetComponent<ChampionAimMelee>();
        championSO = champion.ChampionSO;
    }

    private void Start() {
        championAttackDamage = championSO.championAttackDamage;
        championAimMelee.OnMeleeAttack += ChampionAimMelee_OnMeleeAttack;
    }

    private void ChampionAimMelee_OnMeleeAttack(object sender, ChampionAimMelee.OnMeleeAttackEventArgs e) {

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, championLayer);

        for (int i = 0; i < enemiesToDamage.Length; i++) {
            enemiesToDamage[i].GetComponent<Champion>().ReceiveDamage(championAttackDamage);
            enemiesToDamage[i].GetComponent<KnockbackFeedback>().MeleeKnockback(this.gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}