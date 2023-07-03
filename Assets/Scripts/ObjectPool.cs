using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ObjectPool : MonoBehaviour {

    [SerializeField] GameObject champion;
    [SerializeField] Transform[] spawnPoints;
    private GameObject[] pool;

    private int poolSize = 5;

    private void Start() {
        LoopManager.Instance.OnStateChanged += LoopManager_OnStateChanged;
        PopulatePool();
    }

    private void LoopManager_OnStateChanged(object sender, LoopManager.OnStateChangedEventArgs e) {
        if (e.state == LoopManager.State.Pause) {
            ActivatePool();
            ResetPool();
        }
        if (e.state == LoopManager.State.Recording) {
            ActivatePool();
            pool[e.loopNumber].SetActive(true);
            pool[e.loopNumber].GetComponent<Champion>().SetSpawnedLoopNumber(e.loopNumber);
        }
        if (e.state == LoopManager.State.Playbacking) {
            ActivatePool();
            ResetPool();
        }
        if (e.state == LoopManager.State.PlaybackEndBuffer || e.state == LoopManager.State.RecordingEndBuffer) {
            
        }
    }

    private void PopulatePool() {
        pool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++) {
            pool[i] = Instantiate(champion, spawnPoints[i].transform.position, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }

    private void ResetPool() {
        // Reset champion positions to spawn points, reset health
        for (int i = 0; i < poolSize; i++) {
            pool[i].transform.position = spawnPoints[i].transform.position;
            pool[i].GetComponent<Champion>().ResetChampionHealth();
        }
    }

    private void ActivatePool() {
        // Activate all components, reset velocity to zero, reset attacks
        for (int i = 0; i < poolSize; i++) {
            pool[i].GetComponent<ChampionAim>().enabled = true;
            pool[i].GetComponent<ChampionMovement>().enabled = true;
            pool[i].GetComponent<ChampionRecPlaybackManager>().enabled = true;
            pool[i].GetComponent<Collider2D>().enabled = true;

            pool[i].GetComponent<ChampionMovement>().ResetVelocity();
            pool[i].GetComponent<ChampionAim>().ResetAttacks();
        }
    }

    private void DeactivatePool() {
        // Deactivate all components
        for (int i = 0; i < poolSize; i++) {
            pool[i].GetComponent<ChampionAim>().enabled = false;
            pool[i].GetComponent<ChampionMovement>().enabled = false;
            pool[i].GetComponent<ChampionRecPlaybackManager>().enabled = false;
        }
    }
}
