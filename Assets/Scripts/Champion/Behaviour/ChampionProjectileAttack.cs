using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChampionProjectileAttack : MonoBehaviour
{

    private ChampionAttack championAttack;
    private Champion champion;
    private ChampionSO championSO;

    [SerializeField] private Animator animator;
    [SerializeField] private Projectile projectile;
    private float championAttackDamage;


    private void Awake() {
        champion = GetComponent<Champion>();
        championAttack = champion.GetComponent<ChampionAttack>();
        championSO = champion.ChampionSO;
    }

    private void Start() {
        championAttackDamage = championSO.championAttack1Damage;
        championAttack.OnAttack += ChampionAim_OnAttack;
    }

    private void ChampionAim_OnAttack(object sender, ChampionAttack.OnAttackEventArgs e) {
            animator.SetTrigger("Attack");
            // Vector3 weaponEndPointPosition = e.weaponEndPointPosition;

            Vector3 attackDir = e.attackDir;

            // Transform bulletTransform = Instantiate(projectile.transform, weaponEndPointPosition, Quaternion.identity);
            //bulletTransform.GetComponent<Projectile>().Setup(attackDir, championAttackDamage);
    }
}