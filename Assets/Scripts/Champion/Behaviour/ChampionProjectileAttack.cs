using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChampionProjectileAttack : MonoBehaviour
{

    private ChampionAim championAim;
    private Champion champion;
    private ChampionSO championSO;

    [SerializeField] private Animator animator;
    [SerializeField] private Projectile projectile;
    private float championAttackDamage;


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
            Vector3 weaponEndPointPosition = e.weaponEndPointPosition;

            Vector3 attackDir = e.attackDir;

            Transform bulletTransform = Instantiate(projectile.transform, weaponEndPointPosition, Quaternion.identity);
            bulletTransform.GetComponent<Projectile>().Setup(attackDir, championAttackDamage);
    }
}