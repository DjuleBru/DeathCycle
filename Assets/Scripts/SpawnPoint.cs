using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject visualSelected;
    [SerializeField] GameObject visualHovered;
    [SerializeField] GameObject visualGreyed;
    ObjectPool objectPool;

    private float recordingTimer = 0f;
    private float spawnPointVisualActiveTime = 1f;

    private bool loopOnPause;
    private bool loopOnRecording;

    private void Awake() {
        objectPool = GetComponentInParent<ObjectPool>();
    }


    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
        visualSelected.SetActive(false);
        visualHovered.SetActive(false);
    }

    private void Update() {
        if (objectPool.SelectedSpawnPoint != this) {
            // The selected spawn point is another spawn point
            visualSelected.SetActive(false);
        }
        if (loopOnRecording && recordingTimer > spawnPointVisualActiveTime) {
            DeactivateVisuals();
        }
    }


    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Pause) {
            loopOnPause = true;
            loopOnRecording = false;
            visualGreyed.SetActive(true);
        } else {
            loopOnPause = false;

            if (e.state == LoopManager.State.Recording && objectPool.SelectedSpawnPoint == this) {
                loopOnRecording = true;
                recordingTimer = 0f;
            } else {
                loopOnRecording = false;
                DeactivateVisuals();
            }
        }
    }

    private void DeactivateVisuals() {
        visualSelected.SetActive(false);
        visualHovered.SetActive(false);
        visualGreyed.SetActive(false);
    }

    public void SetSelectedSpawnPoint() {
        visualSelected.SetActive(true);
        objectPool.SetSelectedSpawnPoint(this);
    }

    public void SetHoveredSpawnPoint() {
            visualHovered.SetActive(true);
    }

    public void UnhoveredSpawnPoint() {
            visualHovered.SetActive(false);
    }


}
