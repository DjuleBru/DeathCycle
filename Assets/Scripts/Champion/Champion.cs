using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Champion : MonoBehaviour {

    [SerializeField] private ChampionSO championSO;
    [SerializeField] private ParticleSystem bloodPS;

    private ChampionRecPlaybackManager championRecPlaybackManager;
    private ChampionMovement championMovement;
    private ChampionAttack championAttack;
    private Rigidbody2D rb;

    private float championHealth;

    private int spawnedLoopNumber;
    public int SpawnedLoopNumber { get { return spawnedLoopNumber; } }
    public ChampionSO ChampionSO { get { return championSO; } }

    public event EventHandler<OnDamageReceivedEventArgs> OnDamageReceived;
    public event EventHandler OnDeath;

    public class OnDamageReceivedEventArgs {
        public float championHealth;
    }

    private void Awake() {
        championRecPlaybackManager = GetComponent<ChampionRecPlaybackManager>();
        championMovement = GetComponent<ChampionMovement>();
        championAttack = GetComponent<ChampionAttack>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start() {
        ResetChampionHealth();
    }

    public void SetSpawnedLoopNumber(int spawnedLoopNumber) {
        this.spawnedLoopNumber = spawnedLoopNumber;
    }

    /*
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Projectile") {
            Projectile incomingProjectile = collider.GetComponent<Projectile>();
            float incomingDamage = incomingProjectile.ProjectileDamage;
            ReceiveDamage(incomingDamage);
        }
    }
    */
    public void ReceiveDamage(float incomingDamage) {
        championHealth -= incomingDamage;

        OnDamageReceived?.Invoke(this, new OnDamageReceivedEventArgs {
            championHealth = championHealth
        });

        Instantiate(bloodPS, transform.position, Quaternion.identity);

        if (championHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        championAttack.enabled = false;
        championMovement.enabled = false;
        championRecPlaybackManager.enabled = false;
        rb.velocity = Vector3.zero;

        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public void ResetChampionHealth() {
        championHealth = championSO.championMaxHealth;
    }

    public float GetChampionHealth() {
        return championHealth;
    }
}
