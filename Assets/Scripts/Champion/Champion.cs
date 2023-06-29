using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Champion : MonoBehaviour
{

    [SerializeField] private ChampionSO championSO;

    private float ChampionHealth;

    private int spawnedLoopNumber;
    public int SpawnedLoopNumber { get { return spawnedLoopNumber; } }
    public ChampionSO ChampionSO { get { return championSO; } }

    private void Start() {
        ChampionHealth = championSO.championMaxHealth;
    }

    public void SetSpawnedLoopNumber(int spawnedLoopNumber) {
        this.spawnedLoopNumber = spawnedLoopNumber;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Projectile") {
            Projectile incomingProjectile = collision.collider.GetComponent<Projectile>();
            float incomingDamage = incomingProjectile.ProjectileDamage;

            ChampionHealth -= incomingDamage;
        };
    }
}
