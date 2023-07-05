using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAnimationManager : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private ChampionMovement championMovement;
    private IChampionAttack iChampionAttack;
    private IChampionSpecial iChampionSpecial;
    private Champion champion;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        championMovement = GetComponent<ChampionMovement>();
        iChampionAttack = GetComponent<IChampionAttack>();
        iChampionSpecial = GetComponent<IChampionSpecial>();
        champion = GetComponent<Champion>();

        iChampionAttack.OnAttack += ChampionAttack_OnAttack;
        iChampionSpecial.OnSpecial += IChampionSpecial_OnSpecial;
        champion.OnDamageReceived += Champion_OnDamageReceived;
        champion.OnDeath += Champion_OnDeath;
    }


    private void Update() {

        #region ANIMATIONREAD
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) {
            championMovement.IsAttacking(true);
        } else {
            championMovement.IsAttacking(false);
        }

        iChampionAttack.IsAttacking3(false);
        iChampionAttack.IsAttacking2(false);
        iChampionAttack.IsAttacking1(false);

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Special")) {
            championMovement.IsSpecialing(true);
        } else {
            championMovement.IsSpecialing(false);
        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack")) {
            championMovement.SetVelocity(Vector3.zero);
        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) {
            iChampionAttack.IsAttacking1(true);
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2")) {
            iChampionAttack.IsAttacking2(true);
            animator.SetBool("WillAttack2", false);
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) {
            iChampionAttack.IsAttacking3(true);
            animator.SetBool("WillAttack3", false);
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
    private void ChampionAttack_OnAttack(object sender, IChampionAttack.OnAttackEventArgs e) {
        if (e.attackCount == 0) {
            animator.SetTrigger("AirAttack");
        }
        if (e.attackCount == 1) {
            animator.SetTrigger("Attack1");
            animator.SetBool("WillAttack2", false);
            animator.SetBool("WillAttack3", false);
        }
        if (e.attackCount == 2) {
            animator.SetTrigger("Attack2");
            animator.SetBool("WillAttack2", true);
            animator.SetBool("WillAttack3", false);
        }
        if (e.attackCount == 3) {
            animator.SetTrigger("Attack3");
            animator.SetBool("WillAttack3", true);
            animator.SetBool("WillAttack2", false);
        }
    }
    #endregion

    #region SPECIAL 
    private void IChampionSpecial_OnSpecial(object sender, IChampionSpecial.OnSpecialEventArgs e) {
        animator.SetTrigger("Special");
    }
    #endregion

    #region HIT
    private void Champion_OnDamageReceived(object sender, Champion.OnDamageReceivedEventArgs e) {
        animator.SetTrigger("Hit");
    }
    #endregion

    #region DEATH
    private void Champion_OnDeath(object sender, System.EventArgs e) {
        animator.Play("Die");
    }
    #endregion
}
