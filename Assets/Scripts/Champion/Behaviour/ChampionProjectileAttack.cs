using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChampionProjectileAttack : MonoBehaviour
{

    private Champion champion;
    private ChampionSO championSO;

    [SerializeField] private Animator animator;
    [SerializeField] private Projectile projectile;

    private void Awake() {
        champion = GetComponent<Champion>();
        championSO = champion.ChampionSO;
    }

    private void Start() {
    }
    /*
    private void ChampionAim_OnAttack(object sender, ChampionAttack.OnAttackEventArgs e) {
            animator.SetTrigger("Attack");
            // Vector3 weaponEndPointPosition = e.weaponEndPointPosition;

            Vector3 attackDir = e.attackDir;

            // Transform bulletTransform = Instantiate(projectile.transform, weaponEndPointPosition, Quaternion.identity);
            //bulletTransform.GetComponent<Projectile>().Setup(attackDir, championAttackDamage);
    }
    */
}