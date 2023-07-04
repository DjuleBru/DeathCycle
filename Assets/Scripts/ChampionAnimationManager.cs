using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAnimationManager : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private ChampionMovement championMovement;
    private ChampionAim championAim;
    private Champion champion;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        championMovement = GetComponent<ChampionMovement>();
        championAim = GetComponent<ChampionAim>();
        champion = GetComponent<Champion>();

        championAim.OnAttack += ChampionAim_OnAttack;
        champion.OnDamageReceived += Champion_OnDamageReceived;
    }

    private void Update() {

        #region ANIMATIONREAD
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) {
            championMovement.IsAttacking(true);
        } else {
            championMovement.IsAttacking(false);
        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack")) {
            championMovement.SetVelocity(Vector3.zero);
        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) {
            championAim.IsAttacking1(true);
            championAim.IsAttacking2(false);
            championAim.IsAttacking3(false);
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2")) {
            championAim.IsAttacking2(true);
            championAim.IsAttacking1(false);
            championAim.IsAttacking3(false);
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) {
            championAim.IsAttacking3(true);
            championAim.IsAttacking2(false);
            championAim.IsAttacking1(false);
        }
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) {
            championAim.IsAttacking3(false);
            championAim.IsAttacking2(false);
            championAim.IsAttacking1(false);
        }
                
        #endregion

        #region MOVEMENT
        if (championMovement.MoveDir != 0) {
            animator.SetBool("IsRunning", true);
        } else {
            animator.SetBool("IsRunning", false);
        }
        #endregion

        #region JUMP
        if (rb.velocity.y > 0) {
            animator.SetBool("JumpingUp", true);
        } else {
            animator.SetBool("JumpingUp", false);
        }
        if (rb.velocity.y < 0) {
            animator.SetBool("JumpingDown", true);
        } else {
            animator.SetBool("JumpingDown", false);
        }

        if (rb.velocity.y == 0) {
            animator.ResetTrigger("AirAttack");
        }
        #endregion
    }

    #region attack
    private void ChampionAim_OnAttack(object sender, ChampionAim.OnAttackEventArgs e) {
        if (e.attackCount == 0) {
            animator.SetTrigger("AirAttack");
        }
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

    #region HIT
    private void Champion_OnDamageReceived(object sender, Champion.OnDamageReceivedEventArgs e) {
        animator.SetTrigger("Hit");
    }
    #endregion
}
