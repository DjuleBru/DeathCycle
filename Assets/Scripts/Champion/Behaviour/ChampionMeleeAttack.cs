using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChampionMeleeAttack : MonoBehaviour
{

    private ChampionAim championAim;
    private Champion champion;
    private ChampionSO championSO;
    private Animator animator;

    [SerializeField] private BoxCollider2D weaponCollider;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    private float championAttackDamage;

    public LayerMask championLayer;

    private void Awake() {
        champion = GetComponent<Champion>();
        championAim = champion.GetComponent<ChampionAim>();
        championSO = champion.ChampionSO;
        animator = GetComponent<Animator>();
    }

    private void Start() {
        championAttackDamage = championSO.championAttackDamage;
       // championAim.OnAttack += ChampionAim_OnAttack;
    }

        /*
        private void ChampionAim_OnAttack(object sender, ChampionAim.OnAttackEventArgs e) {

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
        */
    }