using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreFlag : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Vector3 initialPosition;
    private Champion championParent;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        initialPosition = transform.position;
    }

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
    }

    public void SetScoreFlagParent(Champion championParent) {
        this.championParent = championParent;
        transform.parent = championParent.transform;
        boxCollider.enabled = false;
    }

    public void RemoveScoreFlagParent() {
        transform.SetParent(null);
        transform.parent = null;
        championParent = null;
        boxCollider.enabled = true;
    }

    public void DisableScoreFlag() {
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    public void EnableScoreFlag() {
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    public void ResetScoreFlagPosition() {
        transform.position = initialPosition;
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Recording || e.state == LoopManager.State.Playbacking || e.state == LoopManager.State.Pause) {
            RemoveScoreFlagParent();
            EnableScoreFlag();
            ResetScoreFlagPosition();
        }
    }
}
