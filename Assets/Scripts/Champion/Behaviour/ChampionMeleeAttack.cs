using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChampionMeleeAttack : MonoBehaviour
{

    private ChampionAim championAim;
    private Champion champion;
    private ChampionSO championSO;

    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    private float championAttackDamage;

    public LayerMask championLayer;

    private void Awake() {
        champion = GetComponent<Champion>();
        championAim = champion.GetComponent<ChampionAim>();
        championSO = champion.ChampionSO;
    }

    private void Start() {
        championAttackDamage = championSO.championAttackDamage;
        championAim.OnShoot += ChampionAim_OnShoot;
    }

    private void ChampionAim_OnShoot(object sender, ChampionAim.OnShootEventArgs e) {

        animator.SetTrigger("Attack");

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, championLayer);

        for(int i = 0; i < enemiesToDamage.Length; i++) {
            enemiesToDamage[i].GetComponent<Champion>().ReceiveDamage(championAttackDamage);
            enemiesToDamage[i].GetComponent<KnockbackFeedback>().MeleeKnockback(this.gameObject);
        }
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}