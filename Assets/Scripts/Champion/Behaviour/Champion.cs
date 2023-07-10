using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Champion : MonoBehaviour {

    [SerializeField] private ChampionSO championSO;
    [SerializeField] private ParticleSystem bloodPS;
    [SerializeField] private Transform scoreFlagHoldPoint;

    private ObjectPool objectPoolParent;
    private ChampionRecPlaybackManager championRecPlaybackManager;
    private ChampionMovement championMovement;
    private IChampionAttack IChampionAttack;
    private Collider2D championCollider;
    private Rigidbody2D rb;

    private float championHealth;
    private int spawnedLoopNumber;

    private ScoreFlag scoreFlag;
    private int flagScore = 1;
    private int manaOrbValue = 1;
    private int playerMaxMana = 5;

    public event EventHandler<OnDamageReceivedEventArgs> OnDamageReceived;
    public event EventHandler OnDeath;

    public class OnDamageReceivedEventArgs {
        public float championHealth;
    }

    public int SpawnedLoopNumber { get { return spawnedLoopNumber; } }
    public ChampionSO ChampionSO { get { return championSO; } }

    private void Awake() {
        championRecPlaybackManager = GetComponent<ChampionRecPlaybackManager>();
        championMovement = GetComponent<ChampionMovement>();
        IChampionAttack = GetComponent<IChampionAttack>();
        rb = GetComponent<Rigidbody2D>();
        championCollider = GetComponent<Collider2D>();
    }
    private void Start() {
        ResetChampionHealth();
    }

    private void Update() {
        KeepScoreFlagOnHoldPoint();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "ScoreFlag") {
            HandleScoreFlag(collider);
        }

        if (collider.tag == "ScoreZone" && scoreFlag != null) {
            HandleScore();
        }

        if (collider.tag == "ManaOrb") {
            HandleManaOrb(collider);
        }

        if (collider.tag == "Projectile") {
            Projectile incomingProjectile = collider.GetComponent<Projectile>();
            float incomingDamage = incomingProjectile.ProjectileDamage;
            ReceiveDamage(incomingDamage);
        }

    }

    private void HandleManaOrb(Collider2D collider) {
        if (objectPoolParent.GetPlayer().PlayerMana <= playerMaxMana) {
            // Player has mana slots left

            collider.GetComponent<ManaOrb>().DisableManaOrb();
            objectPoolParent.GetPlayer().IncreaseMana(manaOrbValue);
        }
    }

    private void HandleScoreFlag(Collider2D collider) {
        ScoreFlag scoreFlag = collider.GetComponent<ScoreFlag>();
        scoreFlag.SetScoreFlagParent(this);
        scoreFlag.transform.position = scoreFlagHoldPoint.position;
        this.scoreFlag = scoreFlag;
    }

    private void HandleScore() {
        scoreFlag.RemoveScoreFlagParent();
        scoreFlag.DisableScoreFlag();
        scoreFlag.ResetScoreFlagPosition();
        RemoveFlagChildren();
        objectPoolParent.GetPlayer().IncreaseScore(flagScore);
    }

    private void KeepScoreFlagOnHoldPoint() {
        if (scoreFlag != null) {
            scoreFlag.transform.position = scoreFlagHoldPoint.position;
            scoreFlag.transform.rotation = scoreFlagHoldPoint.rotation;
        }
    }

    public void SetSpawnedLoopNumber(int spawnedLoopNumber) {
        this.spawnedLoopNumber = spawnedLoopNumber;
    }

    public void ReceiveDamage(float incomingDamage) {

        if (championHealth > 0) {
            championHealth -= incomingDamage;

            OnDamageReceived?.Invoke(this, new OnDamageReceivedEventArgs {
                championHealth = championHealth
            });

            Instantiate(bloodPS, transform.position, Quaternion.identity);
        }

        if (championHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        rb.velocity = Vector3.zero;

        IChampionAttack.DisableAttacks();
        championMovement.enabled = false;
        championRecPlaybackManager.enabled = false;

        if (scoreFlag != null) {
            scoreFlag.RemoveScoreFlagParent();
            RemoveFlagChildren();
        }

        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public void ResetChampionHealth() {
        championHealth = championSO.championMaxHealth;
    }

    public float GetChampionHealth() {
        return championHealth;
    }

    public ObjectPool GetObjectPoolParent() {
        return objectPoolParent;
    }

    public void SetObjectPoolParent(ObjectPool objectPool) {
        objectPoolParent = objectPool;
    }

    public void RemoveFlagChildren() {
        if (scoreFlag != null) {
            scoreFlag = null;
        }
    }
}
