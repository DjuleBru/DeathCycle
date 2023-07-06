using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaOrb : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;

    private void Awake() {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
    }

    public void DisableManaOrb() {
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
    }

    private void EnableManaOrb() {
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
    }


    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Recording || e.state == LoopManager.State.Playbacking || e.state == LoopManager.State.Pause) {
            EnableManaOrb();
        }
    }

}
