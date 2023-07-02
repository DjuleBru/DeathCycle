using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAnimationManager : MonoBehaviour
{
    private Animator animator;
    private ChampionMovement championMovement;
    private ChampionAimMelee championAimMelee;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
        championMovement = GetComponent<ChampionMovement>();
        championAimMelee = GetComponent<ChampionAimMelee>();
        championAimMelee.OnMeleeAttack += ChampionAimMelee_OnMeleeAttack;
    }


    private void Update() {

        #region MOVEMENT
        if (championMovement.MoveDir != 0) {
            animator.SetBool("IsRunning", true);
            if (championMovement.MoveDir >= 0) {
                gameObject.transform.localScale = new Vector3 (1, 1, 1);
            } else {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }

        } else {
            animator.SetBool("IsRunning", false);
        }
        #endregion
    }

    #region attack
    private void ChampionAimMelee_OnMeleeAttack(object sender, ChampionAimMelee.OnMeleeAttackEventArgs e) {
        if (e.attackCount == 1) {
            animator.SetTrigger("Attack1");
            animator.SetBool("IsAttacking2", false);
            animator.SetBool("IsAttacking3", false);
        }
        if (e.attackCount == 2) {
            animator.SetTrigger("Attack2");
            animator.SetBool("IsAttacking2", true);
            animator.SetBool("IsAttacking3", false);
        }
        if (e.attackCount == 3) {
            animator.SetTrigger("Attack3");
            animator.SetBool("IsAttacking3", true);
        }
    }
    #endregion
}