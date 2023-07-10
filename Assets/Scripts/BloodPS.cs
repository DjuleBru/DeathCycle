using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPS : MonoBehaviour
{
    private float psLifetime = 0.5f;

    private void Update() {
        psLifetime -= Time.deltaTime;
        if (psLifetime <= 0 ) {
            Destroy(this.gameObject);
        }
    }

}
