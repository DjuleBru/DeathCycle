using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 shootDir;
    private float projectileLifeTime = 2f;
    private float projectileSpeed = 75f;
    private float projectileDamage;

    public float ProjectileDamage { get { return projectileDamage; } }

    private bool projectileHit = false;

    public void Setup(Vector3 shootDir, float projectileDamage) {
        this.projectileDamage = projectileDamage;
        this.shootDir = shootDir;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(shootDir));
        Destroy(gameObject, projectileLifeTime);
    }

    private void Update() {
        projectileLifeTime -= Time.deltaTime;
        if (projectileHit || projectileLifeTime < 0) {
            Destroy(gameObject);
        } else {
            transform.position += shootDir.normalized * Time.deltaTime * projectileSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        // Projectile hit something
        projectileHit = true;
    }
}
