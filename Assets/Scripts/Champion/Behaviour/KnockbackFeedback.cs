using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackFeedback : MonoBehaviour
{
    private Champion champion;
    private Rigidbody2D rb;
    [SerializeField] private float knockbackStrength = 15f;
    private float knockbackDelay = 0.15f;

    private void Awake() {
        champion = GetComponent<Champion>();
        rb = GetComponent<Rigidbody2D>();

        champion.OnDamageReceived += Champion_OnDamageReceived;
    }

    public void MeleeKnockback(GameObject sender) {
        Vector2 directionNormalized = (transform.position - sender.transform.position).normalized;
        rb.AddForce(directionNormalized * knockbackStrength, ForceMode2D.Impulse);
    }

    public void ProjectileKnockback(Vector2 knockbackDir) {
        Vector2 knockbackDirNormalized = knockbackDir.normalized;
        rb.AddForce(knockbackDirNormalized * knockbackStrength, ForceMode2D.Impulse);
    }

    private void Champion_OnDamageReceived(object sender, Champion.OnDamageReceivedEventArgs e) {

    }
}
