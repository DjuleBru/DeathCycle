using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChampionMeleeAttack : MonoBehaviour
{

<<<<<<< HEAD
    private ChampionAim championAim;
=======
    private ChampionAimMelee championAimMelee;
>>>>>>> parent of e2851f3 (Revert "Initial commit")
    private Champion champion;
    private ChampionSO championSO;

    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    private float championAttackDamage;

    public LayerMask championLayer;

    private void Awake() {
        champion = GetComponent<Champion>();
<<<<<<< HEAD
        championAim = champion.GetComponent<ChampionAim>();
=======
        championAimMelee = champion.GetComponent<ChampionAimMelee>();
>>>>>>> parent of e2851f3 (Revert "Initial commit")
        championSO = champion.ChampionSO;
    }

    private void Start() {
        championAttackDamage = championSO.championAttackDamage;
<<<<<<< HEAD
        championAim.OnShoot += ChampionAim_OnShoot;
    }

    private void ChampionAim_OnShoot(object sender, ChampionAim.OnShootEventArgs e) {

        animator.SetTrigger("Attack");

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, championLayer);

        for(int i = 0; i < enemiesToDamage.Length; i++) {
=======
        championAimMelee.OnMeleeAttack += ChampionAimMelee_OnMeleeAttack;
    }

    private void ChampionAimMelee_OnMeleeAttack(object sender, ChampionAimMelee.OnMeleeAttackEventArgs e) {

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, championLayer);

        for (int i = 0; i < enemiesToDamage.Length; i++) {
>>>>>>> parent of e2851f3 (Revert "Initial commit")
            enemiesToDamage[i].GetComponent<Champion>().ReceiveDamage(championAttackDamage);
            enemiesToDamage[i].GetComponent<KnockbackFeedback>().MeleeKnockback(this.gameObject);
        }
    }

<<<<<<< HEAD

=======
>>>>>>> parent of e2851f3 (Revert "Initial commit")
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}